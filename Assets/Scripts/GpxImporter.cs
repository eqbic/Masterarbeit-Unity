using System.IO;
using System.Text;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "gpx")]
public class GpxImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var so = ScriptableObject.CreateInstance<GpxData>();
        var text = File.ReadAllText(ctx.assetPath, Encoding.UTF8);
        so.Init(text);
        ctx.AddObjectToAsset(ctx.assetPath, so);
        ctx.SetMainObject(so);
    }
}