// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("VdrtG4mWmyPnJ3PG2r1LhRH43dTgEi30GmXy0mMPaoRs2cJJBAf0OhKRn5CgEpGakhKRkZAQew7ti9OXeRVeIlue4mWoTR/3WQeCAkwah+UTx2G+ycm5AeCyXzvEM6r+2sKE26ASkbKgnZaZuhbYFmedkZGRlZCTwugQ289bStQStpqhZADzYXXhDedUsR+jtdQfBpPLKQwV2QdfxEr8OllhTz0WZthfytuFRjL8lvyeqwp6EmEsB9RwRXiTk+82wXkntpmWFzLLI+LALr5yrvOPNCT3SXvzJcLpRM8CptCcXWYdNe1cccu9802H1O4I4pwrOHCaPfMkvnnnhAOc87xlfMoz+CTw9kSInOGhc1PDz7bYLxaFJOYt6+yJmGP9xZKTkZCR");
        private static int[] order = new int[] { 13,6,6,7,6,13,9,8,10,10,10,13,13,13,14 };
        private static int key = 144;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
