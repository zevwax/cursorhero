namespace ZevWaxGames.CursorHero
{
    public struct Glyph
    {
        public bool IsAlly;
        public float Weight;
        public float Size;
        public bool IsBouncy;
        public bool IsPiercing;
        public float Speed;

        public Glyph(bool isAlly, float weight, float size, bool isBouncy, bool isPiercing, float speed)
        {
            IsAlly = isAlly;
            Weight = weight;
            Size = size;
            IsBouncy = isBouncy;
            IsPiercing = isPiercing;
            Speed = speed;
        }
    }
}