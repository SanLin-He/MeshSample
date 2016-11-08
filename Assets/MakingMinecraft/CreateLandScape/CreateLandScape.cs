using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Block
{
    public int type;//sand grass or snow
    public bool vis;//是否可见

    public GameObject block;
    public Block(int t, bool v, GameObject b)
    {
        this.type = t;
        this.vis = v;
        this.block = b;
    }
}
//一： 地貌
//1.创建平面
//2.使用随机
//3.使用PerlinNoise
//4.不同的高度不同的地貌
//5.碰撞检测删除地貌块

//6.内存整个世界空间的block
//7.显示删除后表面以下（被遮挡）的block
//二：云
//1.crawler算法算云的分布
//2.提高地表heightoffset(以便于丰富地表以下的层)

//三：创建宝石矿,注意边界条件

//四：创建洞穴
//五：添加石块

public class CreateLandScape : MonoBehaviour
{

    public static int width = 128;
    public static int depth = 128;
    public static int height = 128;

    public int heightScale = 20;//
    public int heightOffset = 100;//高度偏移
    public float detailScale = 30.0f;//detailScale 越大越平滑，越小地貌变化越丰富（可能会产生空洞）

    public GameObject grassBlock;
    public GameObject sandBlock;
    public GameObject snowBlock;

    public GameObject CloudBlock;

    public GameObject diamondBlock;

    Block[,,] worldBlocks = new Block[width, height, depth];
    void Start()
    {
        DrawLandScape();

        DrawClouds(20, 100);
        DigCaves(5, 500);
    }

    void DrawLandScape()
    {
        //pernoise 是不变的，随机的话做个种子
        int seed = (int)(Network.time * 0.05f);

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                //int y = 0;
                //int y = (int) Random.Range(0, 10);
                int y = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (z + seed) / detailScale) * heightScale) + heightOffset; //查找pernoise
                Vector3 blockPos = new Vector3(x, y, z);

                CreateLandBlock(y, blockPos, true);
                while (y > 0)
                {
                    y--;
                    blockPos = new Vector3(x, y, z);
                    CreateLandBlock(y, blockPos, false);
                }
            }
        }
    }

    void DrawClouds(int numClouds, int cSize)
    {
        for (int i = 0; i < numClouds; i++)//天上几朵云
        {
            int xpos = Random.Range(0, width);//含头不含尾
            int zpos = Random.Range(0, depth);
            for (int j = 0; j < cSize; j++)//梅朵云的大小
            {
                Vector3 blockPos = new Vector3(xpos, height - 1, zpos);//世界的最顶层
                if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                {
                    GameObject block = (GameObject)Instantiate(CloudBlock, blockPos, Quaternion.identity);
                    worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(4, true, block);
                }
                else
                {
                    j--;//保证大小一定是csize
                }

                //crawler算法
                xpos += Random.Range(-1, 2);
                zpos += Random.Range(-1, 2);
                if (xpos < 0 || xpos >= width)
                {
                    //xpos = (int)(width * 0.5f);
                    xpos = Random.Range(0, width);
                }
                if (zpos < 0 || zpos >= depth)
                {
                    //zpos = (int)(depth * 0.5f);
                    zpos = Random.Range(0, depth);
                }

            }
        }
    }


    void DigCaves(int numMines, int mSize)
    {
        int holeSize = 2;
        for (int i = 0; i < numMines; i++)
        {
            int xpos = Random.Range(10, width - 10);
            int ypos = Random.Range(10, height - heightScale);//height = 128,heightscal = 20(地貌100~120,120~127是天空,10~108是地下)
            int zpos = Random.Range(10, depth - 10);

            for (int j = 0; j < mSize; j++)
            {
                for (int x = -holeSize; x <= holeSize; x++)
                    for (int y = -holeSize; y <= holeSize; y++)
                        for (int z = -holeSize; z <= holeSize; z++)
                        {
                            if (!(x == 0 && y == 0 && z == 0))
                            {
                                Vector3 blockPos = new Vector3(xpos + x, ypos + y, zpos + z);
                                Block block = worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z];
                                if (block != null)
                                    if (block.block != null)
                                        Destroy(block.block);
                                worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;
                            }
                        }

                //crawler算法
                xpos += Random.Range(-1, 2);
                ypos += Random.Range(-1, 2);
                zpos += Random.Range(-1, 2);
                if (xpos < holeSize || xpos >= width - holeSize) xpos = width / 2;
                if (ypos < holeSize || ypos >= width - holeSize) ypos = width / 2;
                if (zpos < holeSize || zpos >= width - holeSize) zpos = width / 2;
            }
        }
        //遍历整个世界空间
        for (int z = 1; z < depth - 1; z++)
            for (int x = 1; x < width - 1; x++)
                for (int y = 1; y < height - 1; y++)
                    if (worldBlocks[x, y, z] == null)
                    {
                        for (int x1 = -1; x1 <= 1; x1++)
                            for (int y1 = -1; y1 <= 1; y1++)
                                for (int z1 = -1; z1 <= 1; z1++)
                                {
                                    if (!(x1 == 0 && y1 == 0 && z1 == 0))
                                    {
                                        Vector3 neighbour = new Vector3(x + x1, y + y1, z + z1);
                                        DrawBlock(neighbour);
                                    }
                                }
                    }
    }
    private void CreateLandBlock(int y, Vector3 blockPos, bool create)
    {
        if (y < 0)
            y = 0;

        GameObject block = null;
        if (y > 15 + heightOffset)
        {
            if (create)
                block = (GameObject)Instantiate(snowBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, y, (int)blockPos.z] = new Block(1, create, block);
        }
        else if (y > 5 + heightOffset)
        {

            if (create)
                block = (GameObject)Instantiate(grassBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, y, (int)blockPos.z] = new Block(2, create, block);
        }
        //else if (y >= heightOffset && y < heightOffset + 2 && Random.Range(0, 100) < 10) //钻石概率层
        //{//不允许往地面以下
        #region 有交叉的层

        else if (y >= 80 && y < heightOffset + 2 && Random.Range(0, 100) < 10) //钻石概率层
        {
            //diamondBlock
            if (create && worldBlocks[(int)blockPos.x, y, (int)blockPos.z] == null)
                block = (GameObject)Instantiate(diamondBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, y, (int)blockPos.z] = new Block(5, create, block);

        }
        //最底层砂石地面
        //else if (y >= heightOffset)
        //{//不允许往地面以下
        else
        {
            if (create && worldBlocks[(int)blockPos.x, y, (int)blockPos.z] == null)
                block = (GameObject)Instantiate(sandBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, y, (int)blockPos.z] = new Block(3, create, block);
        }
        #endregion
    }

    private void DrawBlock(Vector3 blockPos)
    {
        if (blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.z >= depth)
            return;
        Block block = worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z];
        if (block != null && !block.vis)
        {
            GameObject newBlock = null;
            block.vis = true;
            if (block.type == 1)
            {
                newBlock = (GameObject)Instantiate(snowBlock, blockPos, Quaternion.identity);
            }
            else if (block.type == 2)
            {
                newBlock = (GameObject)Instantiate(grassBlock, blockPos, Quaternion.identity);
            }
            else if (block.type == 3)
            {
                newBlock = (GameObject)Instantiate(sandBlock, blockPos, Quaternion.identity);
            }
            else if (block.type == 5)
            {
                newBlock = (GameObject)Instantiate(diamondBlock, blockPos, Quaternion.identity);
            }
            else
            {
                block.vis = false;
            }
            if (newBlock != null)
                block.block = newBlock;
        }
    }

    void Update()
    {
        Digge();
        PlaceBlock();
    }

    private void Digge()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Vector3 blockPos = hit.transform.position;
                //不能删除地面
                //if ((int)blockPos.y <= heightOffset)//不允许往地面以下
                if ((int)blockPos.y <= 0)//往地面以下，但不超过世界盒子
                    return;
                if (blockPos.x <= 0 || blockPos.x >= width - 1 || blockPos.y <= 0 || blockPos.y >= height - 1 || blockPos.z <= 0 || blockPos.x >= depth - 1)
                    return;

                bool isDestroied = hit.collider.gameObject.GetComponent<BlockManager>().RegisterHit();
                if (isDestroied)
                {
                    worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;
                    Destroy(hit.transform.gameObject);
                    for (int x = -1; x <= 1; x++)
                        for (int y = -1; y <= 1; y++)
                            for (int z = -1; z <= 1; z++)
                            {
                                if (!(x == 0 && y == 0 && z == 0))
                                {
                                    Vector3 neighbour = new Vector3(blockPos.x + x, blockPos.y + y, blockPos.z + z);
                                    DrawBlock(neighbour);
                                }
                            }
                }
            }
        }
    }

    private void PlaceBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Vector3 blockPos = hit.transform.position;
                
                //要创建的block的坐标计算
                #region 这样做并不是很好
                //Vector3 hitVector = blockPos - hit.point;

                //hitVector.x = Mathf.Abs(hitVector.x);
                //hitVector.y = Mathf.Abs(hitVector.y);
                //hitVector.z = Mathf.Abs(hitVector.z);

                //if (hitVector.x > hitVector.z && hitVector.x > hitVector.y)
                //    blockPos.x -= Mathf.RoundToInt(ray.direction.x);


                //else if (hitVector.y > hitVector.x && hitVector.y > hitVector.z)
                //    blockPos.y -= Mathf.RoundToInt(ray.direction.y);


                //else
                //    blockPos.z -= Mathf.RoundToInt(ray.direction.z); 
                #endregion

                //这样做要好一些
                Vector3 hitVector = blockPos + hit.normal;
                blockPos.x = (int)Math.Round(hitVector.x, MidpointRounding.AwayFromZero);
                blockPos.y = (int)Math.Round(hitVector.y, MidpointRounding.AwayFromZero);
                blockPos.z = (int)Math.Round(hitVector.z, MidpointRounding.AwayFromZero);

                CreateLandBlock((int)blockPos.y, blockPos, true);
                CheckObscuredNeighbours(blockPos);

            }
        }
    }

    private void CheckObscuredNeighbours(Vector3 blockPos)
    {
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                    {
                        Vector3 neighour = new Vector3(blockPos.x + x, blockPos.y + y, blockPos.z + z);
                        //if it is outside the map
                        if (neighour.x < 1 || neighour.x > width - 2 ||
                            neighour.y < 1 || neighour.y > height - 2 ||
                            neighour.z < 1 || neighour.z > depth - 2) continue;

                        if (worldBlocks[(int)neighour.x, (int)neighour.y, (int)neighour.z] != null)
                        {
                            if (NeighbourCount(neighour) == 26)//3*3*3 -1
                            {
                                Destroy(worldBlocks[(int)neighour.x, (int)neighour.y, (int)neighour.z].block);
                                worldBlocks[(int)neighour.x, (int)neighour.y, (int)neighour.z] = null;
                            }
                        }
                    }
                }
    }

    private int NeighbourCount(Vector3 neighour)
    {
        int count = 0;
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                    {
                        if (worldBlocks[(int)neighour.x + x, (int)neighour.y + y, (int)neighour.z + z] != null /*&& worldBlocks[(int)neighour.x, (int)neighour.y, (int)neighour.z].vis  && worldBlocks[(int)neighour.x, (int)neighour.y, (int)neighour.z].block != null*/)
                        {
                            count++;
                        }
                    }
                }
        return count;
    }
}
