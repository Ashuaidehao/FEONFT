namespace FEOTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var leafs = new string[] { "01", "02", "03", "04", "05", "26986e5df44c8e7bbe3fa0a75d7d268c53889642" }.Select(s => s.HexToBytes().Sha256()).ToArray();

            var merkle = new MerkleTree(leafs);

            merkle.Print();

            var leaf = leafs[^1];
            var prove = merkle.MakeProve(leaf);
            prove.ForEach(p=>Console.WriteLine($"{p.ToHex()}"));

            Console.WriteLine($"leaf:{leaf.ToHex()}");
            Console.WriteLine("Hello, World!");
        }
    }
}