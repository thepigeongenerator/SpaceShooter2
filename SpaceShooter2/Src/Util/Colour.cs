using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace SpaceShooter2.Src.Util;

// Monogame has a default class named "Color", I made a copy which is this class named "Colour" :3
[StructLayout(LayoutKind.Explicit)]
public struct Colour
{
    // I even improved it by making it use union logic, instead of the other rubbish :3
    [FieldOffset(0)] public uint packed;
    [FieldOffset(0)] public byte r;
    [FieldOffset(1)] public byte g;
    [FieldOffset(2)] public byte b;
    [FieldOffset(3)] public byte a;

    public static Colour Transparent => new(0u);
    public static Colour AliceBlue => new(4294965488u);
    public static Colour AntiqueWhite => new(4292340730u);
    public static Colour Aqua => new(4294967040u);
    public static Colour Aquamarine => new(4292149119u);
    public static Colour Azure => new(4294967280u);
    public static Colour Beige => new(4292670965u);
    public static Colour Bisque => new(4291093759u);
    public static Colour Black => new(4278190080u);
    public static Colour BlanchedAlmond => new(4291685375u);
    public static Colour Blue => new(4294901760u);
    public static Colour BlueViolet => new(4293012362u);
    public static Colour Brown => new(4280953509u);
    public static Colour BurlyWood => new(4287084766u);
    public static Colour CadetBlue => new(4288716383u);
    public static Colour Chartreuse => new(4278255487u);
    public static Colour Chocolate => new(4280183250u);
    public static Colour Coral => new(4283465727u);
    public static Colour CornflowerBlue => new(4293760356u);
    public static Colour Cornsilk => new(4292671743u);
    public static Colour Crimson => new(4282127580u);
    public static Colour Cyan => new(4294967040u);
    public static Colour DarkBlue => new(4287299584u);
    public static Colour DarkCyan => new(4287335168u);
    public static Colour DarkGoldenrod => new(4278945464u);
    public static Colour DarkGray => new(4289309097u);
    public static Colour DarkGreen => new(4278215680u);
    public static Colour DarkKhaki => new(4285249469u);
    public static Colour DarkMagenta => new(4287299723u);
    public static Colour DarkOliveGreen => new(4281297749u);
    public static Colour DarkOrange => new(4278226175u);
    public static Colour DarkOrchid => new(4291572377u);
    public static Colour DarkRed => new(4278190219u);
    public static Colour DarkSalmon => new(4286224105u);
    public static Colour DarkSeaGreen => new(4287347855u);
    public static Colour DarkSlateBlue => new(4287315272u);
    public static Colour DarkSlateGray => new(4283387695u);
    public static Colour DarkTurquoise => new(4291939840u);
    public static Colour DarkViolet => new(4292018324u);
    public static Colour DeepPink => new(4287829247u);
    public static Colour DeepSkyBlue => new(4294950656u);
    public static Colour DimGray => new(4285098345u);
    public static Colour DodgerBlue => new(4294938654u);
    public static Colour Firebrick => new(4280427186u);
    public static Colour FloralWhite => new(4293982975u);
    public static Colour ForestGreen => new(4280453922u);
    public static Colour Fuchsia => new(4294902015u);
    public static Colour Gainsboro => new(4292664540u);
    public static Colour GhostWhite => new(4294965496u);
    public static Colour Gold => new(4278245375u);
    public static Colour Goldenrod => new(4280329690u);
    public static Colour Gray => new(4286611584u);
    public static Colour Green => new(4278222848u);
    public static Colour GreenYellow => new(4281335725u);
    public static Colour Honeydew => new(4293984240u);
    public static Colour HotPink => new(4290013695u);
    public static Colour IndianRed => new(4284243149u);
    public static Colour Indigo => new(4286709835u);
    public static Colour Ivory => new(4293984255u);
    public static Colour Khaki => new(4287424240u);
    public static Colour Lavender => new(4294633190u);
    public static Colour LavenderBlush => new(4294308095u);
    public static Colour LawnGreen => new(4278254716u);
    public static Colour LemonChiffon => new(4291689215u);
    public static Colour LightBlue => new(4293318829u);
    public static Colour LightCoral => new(4286611696u);
    public static Colour LightCyan => new(4294967264u);
    public static Colour LightGoldenrodYellow => new(4292016890u);
    public static Colour LightGray => new(4292072403u);
    public static Colour LightGreen => new(4287688336u);
    public static Colour LightPink => new(4290885375u);
    public static Colour LightSalmon => new(4286226687u);
    public static Colour LightSeaGreen => new(4289376800u);
    public static Colour LightSkyBlue => new(4294626951u);
    public static Colour LightSlateGray => new(4288252023u);
    public static Colour LightSteelBlue => new(4292789424u);
    public static Colour LightYellow => new(4292935679u);
    public static Colour Lime => new(4278255360u);
    public static Colour LimeGreen => new(4281519410u);
    public static Colour Linen => new(4293325050u);
    public static Colour Magenta => new(4294902015u);
    public static Colour Maroon => new(4278190208u);
    public static Colour MediumAquamarine => new(4289383782u);
    public static Colour MediumBlue => new(4291624960u);
    public static Colour MediumOrchid => new(4292040122u);
    public static Colour MediumPurple => new(4292571283u);
    public static Colour MediumSeaGreen => new(4285641532u);
    public static Colour MediumSlateBlue => new(4293814395u);
    public static Colour MediumSpringGreen => new(4288346624u);
    public static Colour MediumTurquoise => new(4291613000u);
    public static Colour MediumVioletRed => new(4286911943u);
    public static Colour MidnightBlue => new(4285536537u);
    public static Colour MintCream => new(4294639605u);
    public static Colour MistyRose => new(4292994303u);
    public static Colour Moccasin => new(4290110719u);
    public static Colour MonoGameOrange => new(4278205671u);
    public static Colour NavajoWhite => new(4289584895u);
    public static Colour Navy => new(4286578688u);
    public static Colour OldLace => new(4293326333u);
    public static Colour Olive => new(4278222976u);
    public static Colour OliveDrab => new(4280520299u);
    public static Colour Orange => new(4278232575u);
    public static Colour OrangeRed => new(4278207999u);
    public static Colour Orchid => new(4292243674u);
    public static Colour PaleGoldenrod => new(4289390830u);
    public static Colour PaleGreen => new(4288215960u);
    public static Colour PaleTurquoise => new(4293848751u);
    public static Colour PaleVioletRed => new(4287852763u);
    public static Colour PapayaWhip => new(4292210687u);
    public static Colour PeachPuff => new(4290370303u);
    public static Colour Peru => new(4282353101u);
    public static Colour Pink => new(4291543295u);
    public static Colour Plum => new(4292714717u);
    public static Colour PowderBlue => new(4293320880u);
    public static Colour Purple => new(4286578816u);
    public static Colour Red => new(4278190335u);
    public static Colour RosyBrown => new(4287598524u);
    public static Colour RoyalBlue => new(4292962625u);
    public static Colour SaddleBrown => new(4279453067u);
    public static Colour Salmon => new(4285694202u);
    public static Colour SandyBrown => new(4284523764u);
    public static Colour SeaGreen => new(4283927342u);
    public static Colour SeaShell => new(4293850623u);
    public static Colour Sienna => new(4281160352u);
    public static Colour Silver => new(4290822336u);
    public static Colour SkyBlue => new(4293643911u);
    public static Colour SlateBlue => new(4291648106u);
    public static Colour SlateGray => new(4287660144u);
    public static Colour Snow => new(4294638335u);
    public static Colour SpringGreen => new(4286578432u);
    public static Colour SteelBlue => new(4290019910u);
    public static Colour Tan => new(4287411410u);
    public static Colour Teal => new(4286611456u);
    public static Colour Thistle => new(4292394968u);
    public static Colour Tomato => new(4282868735u);
    public static Colour Turquoise => new(4291878976u);
    public static Colour Violet => new(4293821166u);
    public static Colour Wheat => new(4289978101u);
    public static Colour White => new(uint.MaxValue);
    public static Colour WhiteSmoke => new(4294309365u);
    public static Colour Yellow => new(4278255615u);
    public static Colour YellowGreen => new(4281519514u);

    public Colour(uint packed)
    {
        r = default;
        g = default;
        b = default;
        a = default;
        this.packed = packed;
    }
    public Colour(byte r, byte g, byte b, byte a)
    {
        packed = default;
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public static implicit operator Colour(Color colour) => new(colour.PackedValue);
    public static explicit operator Color(Colour colour) => new(colour.packed);
}
