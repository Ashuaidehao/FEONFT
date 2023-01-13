namespace FEOTestApp;

public class MerkleTree
{
    public byte[] Root { get; set; }
    public byte[][] Leafs { get; set; }
    public List<MerkleNode[]> NodeLayers { get; set; }
    public MerkleTree(byte[][] leafs)
    {
        Leafs = leafs;
        ProcessRoot(leafs);
    }


    public void ProcessRoot(byte[][] leafs)
    {
        var nodeLayers = new List<MerkleNode[]>();
        var nodes = leafs.Select(l => new MerkleNode() { Data = l }).ToArray();

        nodeLayers.Add(nodes);
        var parents = Build(nodes);
        while (parents.Length != 1)
        {
            nodeLayers.Insert(0, parents);
            parents = Build(parents);
        }
        nodeLayers.Insert(0, parents);
        Root = parents[0].Data;
        NodeLayers = nodeLayers;
    }


    public MerkleNode[] Build(MerkleNode[] nodes)
    {
        var parents = new MerkleNode[(nodes.Length + 1) / 2];
        for (int i = 0; i < parents.Length; i++)
        {
            var leftIndex = i * 2;
            var rightIndex = leftIndex + 1;
            parents[i] = new MerkleNode() { Left = nodes[leftIndex] };
            nodes[leftIndex].Parent = parents[i];
            if (rightIndex < nodes.Length)
            {
                parents[i].Right = nodes[rightIndex];
                nodes[rightIndex].Parent = parents[i];
            }
            else
            {
                parents[i].Right = parents[i].Left;
            }
            parents[i].Data = HashPair(parents[i].Left, parents[i].Right);
        }
        return parents;
    }

    public byte[] HashPair(MerkleNode left, MerkleNode right)
    {
        var data= left.Data.LessThan(right.Data)
            ? left.Data.Concat(right.Data).ToArray()
            : right.Data.Concat(left.Data).ToArray();

        //Console.WriteLine($"HashPair:{data.ToHex()}");
        return data.Sha256();
    }


    public void Print()
    {
        Print(NodeLayers[0][0]);
    }

    public void Print(MerkleNode node, int layer = 0, string prefix = "")
    {
        var s = new string(' ', 2);
        //var h = new string('-', layer);
        if (layer == 0)
        {
            Console.WriteLine($"{node.Data.ToHex()}");
        }
        else
        {
            Console.WriteLine($"{prefix}----{node.Data.ToHex()}");

        }
        if (node.Left == null) return;
        var nextLayer = layer + 1;
        Print(node.Left, nextLayer, $"{prefix}{s}|");
        //Console.WriteLine($"{prefix}{s}|");
        Print(node.Right, nextLayer, $"{prefix}{s}|");
    }


    public List<byte[]> MakeProve(byte[] leaf)
    {
        var prove = new List<byte[]>();
        var leafNode = NodeLayers[^1].FirstOrDefault(n => n.Data.SequenceEqual(leaf));
        var currentNode = leafNode;
        while (currentNode?.Parent != null)
        {
            var parent = currentNode.Parent;
            var another = parent.Left == currentNode ? parent.Right : parent.Left;
            prove.Add(another.Data);
            currentNode = parent;
        }
        return prove;
    }
}