using System;
using System.Linq;

namespace PasswordGenerator
{
    public class RCC5
    {
        byte[] S = new Byte[256];
        byte[] key;
        int x = 0;
        int y = 0;
        public RCC5(byte[] key)
        {
            if (key.Length < 6)
            {
                throw new tooShortKeyException("Ключ не может быть короче 6 символов!");
            }
            init(key);

            this.key = key;

        }
        private byte keyItem()
        {
            x = (x + 1) % 256;
            y = (y + S[x]) % 256;

            S.Swap<byte>(x, y);
            return S[(S[x] + S[y]) % 256];
        }
        public void init(byte[] key)
        {
            int keylenth = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + key[i % keylenth]) % 256;
                S.Swap<byte>(i, j);
            }
        }

        public byte[] Encode(byte[] dataB)
        {
            byte[] lol = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                lol[i] = key[S.Length % (key.Length - i)];
            }

            byte[] newdata = new byte[dataB.Length + lol.Length];
            int[] indexes = new int[4];
            for (int i = 0; i < 4; i++)
            {
                int index = S.Length % (newdata.Length - i);
                if (indexes.Contains(index))
                {
                    for (int ii = 0; ii < newdata.Length; ii++)
                    {
                        if (indexes.Contains(ii)) { continue; }
                        else { indexes[i] = ii; }
                    }
                }
                else
                {
                    indexes[i] = index;
                }

            }
            int a = 0;
            for (int i = 0; i < newdata.Length; i++)
            {
                bool isBreak = false;
                for (int ii = 0; ii < indexes.Length; ii++)
                {

                    if (i == indexes[ii]) { isBreak = true; break; }
                }
                if (isBreak) { continue; }
                if (a >= dataB.Length) { break; }
                newdata[i] = dataB[a];
                a++;
            }
            for (int i = 0; i < indexes.Length; i++)
            {
                newdata[indexes[i]] = lol[i];
            }

            byte[] data = newdata.Take(newdata.Length).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                cipher[i] = (byte)(data[i] ^ keyItem());
            }
            return cipher;

        }
        public byte[] Decode(byte[] dataB)
        {
            byte[] data = dataB.Take(dataB.Length).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                cipher[i] = (byte)(data[i] ^ keyItem());
            }
            byte[] lol = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                lol[i] = key[S.Length % (key.Length - i)];
            }
            int[] indexes = new int[4];
            for (int i = 0; i < 4; i++)
            {
                int index = S.Length % (cipher.Length - i);
                if (indexes.Contains(index))
                {
                    for (int ii = 0; ii < cipher.Length; ii++)
                    {
                        if (indexes.Contains(ii)) { continue; }
                        else { indexes[i] = ii; }
                    }
                }
                else
                {
                    indexes[i] = index;
                }

            }

            for (int i = 0; i < indexes.Length; i++)
            {
                if (cipher[indexes[i]] != lol[i])
                {
                    throw new InvalidKeyException("Ключ неверный или файл повреждён.");
                }
            }
            byte[] original = new byte[cipher.Length - lol.Length];
            int a = 0;
            for (int i = 0; i < cipher.Length; i++)
            {
                bool isBreak = false;
                for (int ii = 0; ii < indexes.Length; ii++)
                {
                    if (i == indexes[ii])
                    {
                        isBreak = true;
                        break;
                    }
                }
                if (isBreak) { continue; }
                if (a >= original.Length) { break; }
                original[a] = cipher[i];
                a++;
            }
            return original;


        }

    }

}
