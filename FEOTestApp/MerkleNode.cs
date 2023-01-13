namespace FEOTestApp;

public class MerkleNode
{
    public MerkleNode Parent { get; set; }
    public MerkleNode Left { get; set; }
    public MerkleNode Right { get; set; }

    public byte[] Data { get; set; }
}