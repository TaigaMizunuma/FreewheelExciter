using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ItemList_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/ItemList.xlsx";
    private static readonly string[] sheetNames = { "ItemList", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/Resources/Data/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_ItemList)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_ItemList));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_ItemList>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new Entity_ItemList.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.recovery = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.category = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.message = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.ex_hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.ex_sp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.ex_str = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.ex_skl = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.ex_spd = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.ex_luk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.ex_def = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.ex_cur = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.ex_move = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.good_effect = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.bad_effect = (cell == null ? "" : cell.StringCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
