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
#if UNITY_2019_3_OR_NEWER
            var targetGuid = proj.GetUnityFrameworkTargetGuid();
#else
            var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif
            if(string.IsNullOrEmpty(proj.FindFileGuidByRealPath("usr/lib/libz.tbd", PBXSourceTree.Sdk)))
                proj.AddFileToBuild(targetGuid, proj.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));
            proj.WriteToFile(projPath);
        }
    }
}
#endif