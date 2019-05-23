using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public int padding;

    public GameObject inventorySpacePrefab;
    public GameObject inventoryWindow;

    public int windowBorder;

    GameObject[,] inventorySpaces;

    public int maxCellDimension; //size of largest possible cell

    public int maxInventoryWidth; //calculated from width of screen
    public int maxInventoryHeight; //calculated from height of screen

    //public int cellBorderWidth;

    public int cellDimension;

    public int numHorizontalCells;
    public int numVerticalCells;

    // Start is called before the first frame update
    void Start()
    {
        maxCellDimension = Screen.width / 20;
        maxInventoryWidth = (int)(Screen.width * 0.5f);
        maxInventoryHeight = (int)(Screen.height * 0.5f);

        //InitUi(new GameObject[4, 3]);
    }

    public void InitUi(GameObject[,] inventory)
    { //sets the ui, GameObject must be of type Item
        gameObject.SetActive(true);
        numHorizontalCells = inventory.GetLength(0);
        numVerticalCells = inventory.GetLength(1);

        inventorySpaces = new GameObject[numHorizontalCells, numVerticalCells];


        int cellWidth = (numHorizontalCells * maxCellDimension);
        int extraWidth = (padding * (numHorizontalCells - 1)) + (windowBorder * 2);
        int totalWidth = cellWidth + extraWidth;

        int cellHeight = (numVerticalCells * maxCellDimension) ;
        int extraHeight = (padding * (numVerticalCells - 1)) + (windowBorder * 2);
        int totalHeight = cellHeight + extraHeight;

        cellDimension = maxCellDimension;

        if (totalWidth > maxInventoryWidth)
        {
            totalWidth = maxInventoryWidth;
            cellDimension = (totalWidth - extraWidth) / numHorizontalCells;
        }

        if (totalHeight > maxInventoryHeight)
        {
            totalHeight = maxInventoryHeight;
            if((totalHeight - extraHeight) / numVerticalCells < cellDimension)
            {
                cellDimension = (totalHeight - extraHeight) / numVerticalCells;
            } 
        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(totalWidth, totalHeight);
        inventoryWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(totalWidth, totalHeight);

        int currentX = windowBorder;
        int currentY = windowBorder;
        
        for(int y = 0; y <= numVerticalCells - 1; y++) //column
        {
            for(int x = 0; x <= numHorizontalCells - 1; x++) //row
            {
                GameObject newSpace = GameObject.Instantiate(inventorySpacePrefab); //create new space
                newSpace.transform.SetParent(inventoryWindow.transform); //add to parent
                newSpace.GetComponent<RectTransform>().localPosition = new Vector2(currentX, currentY); //align with space under parent
                newSpace.GetComponent<RectTransform>().sizeDelta = new Vector2(cellDimension, cellDimension);
                inventorySpaces[x, y] = newSpace;

                currentX = currentX + padding + cellDimension;
            }
            currentX = windowBorder;
            currentY = currentY + padding + cellDimension;
        }
    }

    public void UpdateItems()
    {
        //inventoryWindow.getChild();

    }

}
