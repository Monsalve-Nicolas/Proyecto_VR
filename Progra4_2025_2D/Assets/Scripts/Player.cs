using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] Turret turret;
    public List<StatInfo> currentStats = new List<StatInfo>();
    [Header("Current Piece")]
    public TankPieceScriptable[] piecesArr = new TankPieceScriptable[7];
    StatType[] statTypesArr = new StatType[6];
    public Color piece_LightColor;

    private void Start()
    {
        UpdateControllerWithTypePiece();
    }
    public void OnTankPieceChange(TankPieceScriptable tankPiece)
    {
        piecesArr[(int)tankPiece.pieceType] = tankPiece;

        Debug.Log("Pieza modificada = " + tankPiece.pieceType);
        Debug.Log("El ID = " + tankPiece.id);
        UpdateControllerWithTypePiece();
    }
    public void UpdateControllerWithTypePiece()
    {
        List<StatInfo> statsInfo = new List<StatInfo>();

        foreach (var piece in piecesArr)
        {
            if(piece != null)
            {
                foreach (var item in piece.statInfo)
                {
                    Debug.Log("YEyyyyyy");
                    StatInfo currentStats = statsInfo.Find(x => x.type == item.type);
                    if (currentStats != null)
                    {
                        currentStats.value = item.value;
                    }
                    else
                    {
                        StatInfo newStats = new StatInfo();
                        newStats.value = item.value;
                        newStats.type = item.type;
                        statsInfo.Add(newStats);
                    }
                }
            }

        }
        currentStats = statsInfo;
    }
    public void UpdateControllers()
    {
        foreach (var stats in currentStats)
        {
            switch (stats.type)
            {
                case StatType.Spd:
                    movement.moveSpd += stats.value;
                    break;
                case StatType.RootSpd:
                    movement.rotateSpd += stats.value;
                    turret.rotateSpd += stats.value;
                    break;
                case StatType.Attack:
                    break;
                case StatType.Defense:
                    break;
                case StatType.Life:
                    break;
                case StatType.BulletSpd:
                    break;
                default:
                    break;
            }
        }

    }
}
