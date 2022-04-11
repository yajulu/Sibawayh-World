// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("uAqJqriFjoGiDsAOf4WJiYmNiIvTO/rYNqZqtuuXLDzvUWPrPdrxXAvfeabR0aEZ+KpHI9wrsubC2pzD+Ao17AJ96sp7F3KcdMHaURwf7CJMqQe7rcwHHovTMRQNwR9H3FLkItcavsiERX4FLfVEadOl61WfzPYQComHiLgKiYKKComJiAhjFvWTy4/a8AjD10NSzAqugrl8GOt5bfkV//qEMyBogiXrPKZh/5wbhOukfWTSCnk0H8xoXWCLi/cu2WE/roGODypNwvUDkY6DO/8/a97CpVOdCeDFzCvgPOjuXJCE+blrS9vXrsA3Dp08QXlXJQ5+wEfSw51eKuSO5IazEmJhDUY6Q4b6fbBVB+9BH5oaVAKf/f418/SRgHvl3YqLiYiJ");
        private static int[] order = new int[] { 0,4,2,6,8,5,8,9,11,11,13,12,13,13,14 };
        private static int key = 136;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
