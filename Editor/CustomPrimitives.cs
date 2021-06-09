// Joakim Linna KimMakesGames@gmail.com 2021-06-09

using UnityEditor;
using UnityEngine;

namespace KimMakesGames.DevTexturePrimitives.Editor
{
    public class CustomPrimitives : MonoBehaviour
    {
        private const string _menuPath = "GameObject/Dev Texture Primitives/";
        private const string _materialPath = "DevTexturePrimitives/Materials/dev_material";

        [MenuItem(_menuPath + "Sphere", false, 10)]
        static void CreateCustomSphere(MenuCommand menuCommand) => Create(menuCommand, PrimitiveType.Sphere);

        [MenuItem(_menuPath + "Capsule", false, 10)]
        static void CreateCustomCapsule(MenuCommand menuCommand) => Create(menuCommand, PrimitiveType.Capsule);

        [MenuItem(_menuPath + "Cylinder", false, 10)]
        static void CreateCustomCylinder(MenuCommand menuCommand) => Create(menuCommand, PrimitiveType.Cylinder);

        [MenuItem(_menuPath + "Cube", false, 10)]
        static void CreateCustomCube(MenuCommand menuCommand) => Create(menuCommand, PrimitiveType.Cube);

        [MenuItem(_menuPath + "Plane", false, 10)]
        static void CreateCustomPlane(MenuCommand menuCommand) => Create(menuCommand, PrimitiveType.Plane);

        [MenuItem(_menuPath + "Quad", false, 10)]
        static void CreateCustomQuad(MenuCommand menuCommand) => Create(menuCommand, PrimitiveType.Quad);

        private static void Create(MenuCommand menuCommand, PrimitiveType primitiveType)
        {
            GameObject go = GameObject.CreatePrimitive(primitiveType);
            go.GetComponent<MeshRenderer>().material = Resources.Load(_materialPath) as Material;
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
            Selection.activeObject = go;
        }
    }
}