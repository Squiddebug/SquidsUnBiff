﻿namespace SquidsUnBiff
{
    public class Utility
    {
        public static string MapResourceTypeId(ushort resourceTypeId)
        {
            //TODO CLEAN UP
            switch (resourceTypeId)
            {
                case 0 :
                    return "res";
                case 1:
                    return "bmp";
                case 2:
                    return "mve";
                case 3:
                    return "tga";
                case 4:
                    return "wav";
                case 6:
                    return "plt";
                case 7:
                    return "ini";
                case 8:
                    return "mp3";
                case 9:
                    return "mpg";
                case 10:
                    return "txt";
                case 11:
                    return "xml";
                case 2000:
                    return "plh";
                case 2001:
                    return "tex";
                case 2002:
                    return "mdl";
                case 2003:
                    return "thg";
                case 2005:
                    return "fnt";
                case 2007:
                    return "lua";
                case 2008:
                    return "slt";
                case 2009:
                    return "nss";
                case 2010:
                    return "ncs";
                case 2011:
                    return "mod";
                case 2012:
                    return "are";
                case 2013:
                    return "set";
                case 2014:
                    return "ifo";
                case 2015:
                    return "bic";
                case 2016:
                    return "wok";
                case 2017:
                    return "2da";
                case 2018:
                    return "tlk";
                case 2022:
                    return "txi";
                case 2023:
                    return "git";
                case 2024:
                    return "bti";
                case 2025:
                    return "uti";
                case 2026:
                    return "btc";
                case 2027:
                    return "utc";
                case 2029:
                    return "dlg";
                case 2030:
                    return "itp";
                case 2031:
                    return "btt";
                case 2032:
                    return "utt";
                case 2033:
                    return "dds";
                case 2034:
                    return "bts";
                case 2035:
                    return "uts";
                case 2036:
                    return "ltr";
                case 2037:
                    return "gff";
                case 2038:
                    return "fac";
                case 2039:
                    return "bte";
                case 2040:
                    return "ute";
                case 2041:
                    return "btd";
                case 2042:
                    return "utd";
                case 2043:
                    return "btp";
                case 2044:
                    return "utp";
                case 2045:
                    return "dft";
                case 2070:
                    return "mdb";
                case 2071:
                    return "say";
                case 2072:
                    return "ttf";
                case 2073:
                    return "ttc";
                case 2074:
                    return "cut";
                case 2075:
                    return "ka";
                case 2076:
                    return "jpg";
                case 2077:
                    return "ico";
                case 2078:
                    return "ogg";
                case 2079:
                    return "spt";
                case 2080:
                    return "spw";
                case 2081:
                    return "wfx";
                case 2110:
                    return "png";
                case 9998:
                    return "bif";
                case 9999:
                    return "key";
                default:
                    return string.Empty;
            }
        }
    }
}
