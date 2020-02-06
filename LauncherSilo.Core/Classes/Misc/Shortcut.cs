using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misc
{
    public class Shortcut
    {
        public string FullName
        {
            get
            {
                if (objShortcut == null)
                {
                    return string.Empty;
                }
                return objShortcut.FullName;
            }
        }
        public string Arguments
        {
            get
            {
                if (objShortcut == null)
                {
                    return string.Empty;
                }
                return objShortcut.Arguments;
            }
            set
            {
                if (objShortcut == null)
                {
                    return;
                }
                if (value == objShortcut.Arguments)
                {
                    return;
                }
                objShortcut.Arguments = value;
                objShortcut.Save();
            }
        }
        public string Description
        {
            get
            {
                if (objShortcut == null)
                {
                    return string.Empty;
                }
                return objShortcut.Description;
            }
            set
            {
                if (objShortcut == null)
                {
                    return;
                }
                if (value == objShortcut.Description)
                {
                    return;
                }
                objShortcut.Description = value;
                objShortcut.Save();
            }
        }
        public string Hotkey
        {
            get
            {
                if (objShortcut == null)
                {
                    return string.Empty;
                }
                return objShortcut.Hotkey;
            }
            set
            {
                if (objShortcut == null)
                {
                    return;
                }
                if (value == objShortcut.Hotkey)
                {
                    return;
                }
                objShortcut.Hotkey = value;
                objShortcut.Save();
            }
        }
        public string IconLocation
        {
            get
            {
                if (objShortcut == null)
                {
                    return string.Empty;
                }
                return objShortcut.IconLocation;
            }
            set
            {
                if (objShortcut == null)
                {
                    return;
                }
                if (value == objShortcut.IconLocation)
                {
                    return;
                }
                objShortcut.IconLocation = value;
                objShortcut.Save();
            }
        }
        public string TargetPath
        {
            get
            {
                if (objShortcut == null)
                {
                    return string.Empty;
                }
                return objShortcut.TargetPath;
            }
            set
            {
                if (objShortcut == null)
                {
                    return;
                }
                if (value == objShortcut.TargetPath)
                {
                    return;
                }
                objShortcut.TargetPath = value;
                objShortcut.Save();
            }
        }
        public int WindowStyle
        {
            get
            {
                if (objShortcut == null)
                {
                    return 0;
                }
                return objShortcut.WindowStyle;
            }
            set
            {
                if (objShortcut == null)
                {
                    return;
                }
                if (value == objShortcut.WindowStyle)
                {
                    return;
                }
                objShortcut.WindowStyle = value;
                objShortcut.Save();
            }
        }
        public string WorkingDirectory
        {
            get
            {
                if (objShortcut == null)
                {
                    return string.Empty;
                }
                return objShortcut.WorkingDirectory;
            }
            set
            {
                if (objShortcut == null)
                {
                    return;
                }
                if (value == objShortcut.WorkingDirectory)
                {
                    return;
                }
                objShortcut.WorkingDirectory = value;
                objShortcut.Save();
            }
        }

        private const string LinkExtension = ".lnk";
        private dynamic objShortcut = null;

        public Shortcut(string linkTo)
        {
            dynamic shell = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));


            string actualLinkName = linkTo;
            if (System.IO.Path.GetExtension(actualLinkName) != LinkExtension)
            {
                actualLinkName = System.IO.Path.ChangeExtension(actualLinkName, LinkExtension);
            }
            objShortcut = shell.CreateShortcut(actualLinkName);
        }
        public void Delete()
        {
            if (!System.IO.File.Exists(FullName))
            {
                return;
            }
            System.IO.File.Delete(FullName);
        }

    }
}
