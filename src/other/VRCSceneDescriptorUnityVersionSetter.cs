#if VRC_SDK_VRCSDK3
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VRC.SDK3.Components;
using ver = System.Version;

[InitializeOnLoad]
internal static class VRCSceneDescriptorUnityVersionSetter
{
    static VRCSceneDescriptorUnityVersionSetter()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        var unityVersion = InternalEditorUtility.GetUnityDisplayVersion();
        ver unityVer = new(unityVersion[..^2]); // skip the f1 at the end of.
//      vou? (with an English accent pretty please!)

        var world = Object.FindFirstObjectByType<VRCSceneDescriptor>(
            FindObjectsInactive.Include
        );

        if (world == null) return;

        if (string.IsNullOrEmpty(world.unityVersion))
        {
            SetUnityVersion(world, unityVersion);
            return;
        }

        ver worldUnityVer = new(world.unityVersion[..^2]); // same as above...
        if (worldUnityVer != unityVer) SetUnityVersion(world, unityVersion);

        static void SetUnityVersion(VRCSceneDescriptor VRCene, string version)
        {
            Undo.RecordObject(VRCene, $"{VRCene.name}.unityVersion changed");
            VRCene.unityVersion = version;
            EditorUtility.SetDirty(VRCene);
        }
    }
}
#endif
