using UnityEngine;

public class TileClickDetector : MonoBehaviour
{
    private TileProperties tileProperties;
    private PawnMover pawnMover;
    public int BoardSize { get; private set; } = 8;



    private void Awake()
    {
        tileProperties = GetComponent<TileProperties>();
        if (PlayerPrefs.HasKey("BoardSize"))
            BoardSize = PlayerPrefs.GetInt("BoardSize");
    }

    private void Start()
    {
        pawnMover = GetComponentInParent<PawnMover>();
    }

    public void ChildPawnClicked()
    {
        OnMouseDown();
    }

    private void OnMouseDown()
    {

        if (!GameManager.Instance.isPlaying)
            return;
        
        PlayerTurn turn = new PlayerTurn(GetComponent<TileProperties>().GetTileIndex().Index(BoardSize));
        if(GameManager.Instance.gameType == GameManager.GameType.VSPLAYERS &&  GameManager.Instance.isPlaying)
            GameManager.Instance.socket.Emit("click", JsonUtility.ToJson(turn));

        if (tileProperties.IsOccupied())
            pawnMover.PawnClicked(tileProperties.GetPawn());
        else
            pawnMover.TileClicked(this.gameObject);
    }

   

    public void ClickTile()
    {
        // OnMouseDown();

        PlayerTurn turn = new PlayerTurn(GetComponent<TileProperties>().GetTileIndex().Index(BoardSize));
      
        if (tileProperties.IsOccupied())
            pawnMover.PawnClicked(tileProperties.GetPawn());
        else
            pawnMover.TileClicked(this.gameObject);
    }
}