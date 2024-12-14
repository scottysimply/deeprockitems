namespace deeprockitems.Content.Upgrades
{
    public class RecipeBinding
    {
        public RecipeBinding(int[] types, int stack) {
            AcceptedTypes = types;
            Stack = stack;
        }
        public int[] AcceptedTypes;
        public int Stack;
    }
}
