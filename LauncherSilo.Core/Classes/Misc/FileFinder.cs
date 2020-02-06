using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.IO.Filesystem.Ntfs;


using Algorithms;
using Win32;
using Misc;

namespace Misc
{

    public class FileFinder
    {
        public event EventHandler OnInitializeComplate;

        public enum eState
        {
            None,
            NotReady,
            Ready,
        }
        private Dictionary<string, HashSet<string>> _IndexDictionary = new Dictionary<string, HashSet<string>>();
        private Attributes _IgnoreAttributes = (Attributes.System | Attributes.Device | Attributes.Temporary);
        private System.Timers.Timer _RebuildTimmer = new System.Timers.Timer(2 * (60 * 60 * 1000));
        
        public bool IsReady
        {
            get
            {
                return _State == eState.Ready;
            }
        }
        public eState State
        {
            get
            {
                return _State;
            }
        }
        private eState _State = eState.None;


        public FileFinder()
        {
            
        }
        public void Initialize()
        {
            BuildFileIndex();
            _RebuildTimmer.Elapsed += RebuildTimmer_Elapsed;
            _RebuildTimmer.Enabled = true;
        }

        private void RebuildTimmer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BuildFileIndex();
        }

        public async Task<List<string>> SearchFileAsync(string searchName, float range, int max, CancellationToken cancelToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            List<string> Result = new List<string>(max);
            if (searchName != string.Empty)
            {
                string searchNameLower = searchName.ToLower();
                await Task.Run(() =>
                {
                    Algorithms.LevenshteinDistance levenshteinDistance = new LevenshteinDistance();
                    var Contains = _IndexDictionary
                    .AsParallel()
                    .WithCancellation(cancelToken)
                    .Where(x => x.Key.Contains(searchNameLower))
                    .OrderBy(x => 1.0 - levenshteinDistance.CalculateNormalizedDistance(searchNameLower, x.Key))
                    .SelectMany(x => x.Value.ToArray());
                    Result.AddRange(Contains.Take(max));
                    if (Result.Count >= max)
                    {
                        return;
                    }
                    if (cancelToken.IsCancellationRequested)
                    {
                        return;
                    }
                    var NotContains = _IndexDictionary
                    .AsParallel()
                    .WithCancellation(cancelToken)
                    .Where(x => !x.Key.Contains(searchNameLower) && (1.0 - levenshteinDistance.CalculateNormalizedDistance(searchName, x.Key)) <= range)
                    .OrderBy(x => 1.0 - levenshteinDistance.CalculateNormalizedDistance(searchNameLower, x.Key))
                    .SelectMany(x => x.Value.ToArray());
                    int RemainCapacity = max - Result.Count;
                    Result.AddRange(NotContains.Take(RemainCapacity));
                    if (cancelToken.IsCancellationRequested)
                    {
                        return;
                    }


                });
            }
            stopwatch.Stop();
            LogStatics.Debug(string.Format("search at {0:F3} s", (float)stopwatch.Elapsed.TotalSeconds));
            return Result;
        }

        private async void BuildFileIndex()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _State = eState.NotReady;
            string[] FixedDrives = GetFixedDriveNames();
            List<Task> PrepareTasks = new List<Task>();
            foreach (var FixedDrive in FixedDrives)
            {

                Task Prepare = Task.Run(() =>
                {
                    try
                    {
                        LogStatics.Debug("begin analyze ntfs drive : {0}", FixedDrive);
                        DriveInfo driveToAnalyze = new DriveInfo(FixedDrive);
                        NtfsReader ntfsReader = new NtfsReader(driveToAnalyze, RetrieveMode.Minimal);
                        IEnumerable<INode> nodes = ntfsReader.GetNodes(driveToAnalyze.Name);
                        return nodes.ToArray();
                    }
                    catch (Exception ex)
                    {
                        LogStatics.Warn("failue analyze ntfs drive : {0} {1}", FixedDrive, ex);
                    }
                    finally
                    {
                        LogStatics.Debug("end analyze ntfs drive : {0}", FixedDrive);
                    }
                    return null;
                }).ContinueWith(task => 
                {
                    try
                    {
                        LogStatics.Debug("begin build ntfs drive : {0}", FixedDrive);
                        BuildIndexDictionary(task.Result, ref _IndexDictionary);
                    }
                    catch(Exception ex)
                    {
                        LogStatics.Warn("failue build ntfs drive : {0} {1}", FixedDrive, ex);
                    }
                    finally
                    {
                        LogStatics.Debug("end build ntfs drive : {0}", FixedDrive);
                    }

                });
                PrepareTasks.Add(Prepare);
            }
            await Task.WhenAll(PrepareTasks);
            GC.Collect();
            _State = eState.Ready;
            RaiseOnInitializeComplate();
            LogStatics.Debug("file finder total file index build time {0} ms", (float)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond);
        }


        private void BuildIndexDictionary(List<INode> SourceIndex, ref Dictionary<string, HashSet<string>> OutDictionary)
        {
            BuildIndexDictionary(SourceIndex.ToArray(), ref OutDictionary);
        }
        private void BuildIndexDictionary(INode[] SourceIndex, ref Dictionary<string, HashSet<string>> OutDictionary)
        {
            try
            {
                foreach (var Source in SourceIndex)
                {
                    if ((Source.Attributes & _IgnoreAttributes) != 0)
                    {
                        continue;
                    }
                    AddIndexDictionary(ref OutDictionary, Source.Name.Normalize().ToLower(), Source.FullName.Normalize().ToLower());
                }
            }
            catch(Exception ex)
            {
                LogStatics.Debug(ex.ToString());
            }
        }
        private void AddIndexDictionary(ref Dictionary<string, HashSet<string>> OutDictionary, string Name, string FullName)
        {
            lock (OutDictionary)
            {
                if (OutDictionary.ContainsKey(Name))
                {
                    OutDictionary[Name].Add(FullName.Normalize());
                }
                else
                {
                    OutDictionary.Add(Name.Normalize(), new HashSet<string>() { FullName.Normalize() });
                }
            }
        }
        private void RemoveIndexDictionary(ref Dictionary<string, HashSet<string>> OutDictionary, string Name, string FullName)
        {
            lock (OutDictionary)
            {
                if (OutDictionary.ContainsKey(Name))
                {
                    OutDictionary[Name].Remove(FullName.Normalize());
                }
            }
        }
        private void ClearIndexDictionary(ref Dictionary<string, HashSet<string>> OutDictionary)
        {
            lock (OutDictionary)
            {
                OutDictionary.Clear();
            }
        }

        private string[] GetFixedDriveNames()
        {
            DriveInfo[] LogicalDrives = DriveInfo.GetDrives();
            List<string> FixedDrives = new List<string>();
            string NTFSDriveFormat = "NTFS";
            foreach (var Drive in LogicalDrives)
            {
                if (Drive.DriveType == DriveType.Fixed)
                {
                    if (Drive.DriveFormat == NTFSDriveFormat)
                    {
                        LogStatics.Debug("add ntfs drive : {0}", Drive.Name);
                        FixedDrives.Add(Drive.Name);
                    }
                }
            }
            return FixedDrives.ToArray();
        }
        private void RaiseOnInitializeComplate()
        {
            if (OnInitializeComplate != null)
            {
                OnInitializeComplate(this, EventArgs.Empty);
            }
        }

    }
}
