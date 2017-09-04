#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Appmetr.Unity.Editor
{
    public class PostProcess
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            // add libz library into project
            var projPath = Path.Combine(Path.Combine(pathToBuiltProject, "Unity-iPhone.xcodeproj"), "project.pbxproj");
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);
            var targetGuid = proj.TargetGuidByName("Unity-iPhone");
            proj.AddFileToBuild(targetGuid, proj.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));
            proj.WriteToFile(projPath);
            
            
            // add appmetrUrl and trackInstallByApp to Info.plist
            var infoPlist = new PlistDocument();
            var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            infoPlist.ReadFromFile(plistPath);
            if (infoPlist.root["appmetrUrl"] == null)
            {
                infoPlist.root.SetString("appmetrUrl", "https://appmetr.com/api");
            }
            if (infoPlist.root["trackInstallByApp"] == null)
            {
                infoPlist.root.SetBoolean("trackInstallByApp", false);
            }
            infoPlist.WriteToFile(plistPath);
        }
    }
}
#endif