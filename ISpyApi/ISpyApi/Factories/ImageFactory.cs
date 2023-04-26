﻿namespace ISpyApi;

public class ImageFactory
{
    private readonly string[] images = new string[]
    {
        "animal_02705.png",
        "animal_0274C.png",
        "animal_1002705.png",
        "animal_100274C.png",
        "animal_1012705.png",
        "animal_1022705.png",
        "animal_102705.png",
        "animal_10274C.png",
        "animal_1032705.png",
        "animal_1042705.png",
        "animal_1052705.png",
        "animal_1062705.png",
        "animal_1072705.png",
        "animal_1082705.png",
        "animal_1092705.png",
        "animal_1102705.png",
        "animal_1112705.png",
        "animal_1122705.png",
        "animal_112705.png",
        "animal_11274C.png",
        "animal_1132705.png",
        "animal_1142705.png",
        "animal_1152705.png",
        "animal_1162705.png",
        "animal_1172705.png",
        "animal_1182705.png",
        "animal_1192705.png",
        "animal_1202705.png",
        "animal_1212705.png",
        "animal_1222705.png",
        "animal_122705.png",
        "animal_12274C.png",
        "animal_1232705.png",
        "animal_12705.png",
        "animal_1274C.png",
        "animal_132705.png",
        "animal_13274C.png",
        "animal_142705.png",
        "animal_14274C.png",
        "animal_152705.png",
        "animal_15274C.png",
        "animal_162705.png",
        "animal_16274C.png",
        "animal_172705.png",
        "animal_17274C.png",
        "animal_182705.png",
        "animal_18274C.png",
        "animal_192705.png",
        "animal_19274C.png",
        "animal_202705.png",
        "animal_20274C.png",
        "animal_212705.png",
        "animal_21274C.png",
        "animal_222705.png",
        "animal_22274C.png",
        "animal_22705.png",
        "animal_2274C.png",
        "animal_232705.png",
        "animal_23274C.png",
        "animal_242705.png",
        "animal_24274C.png",
        "animal_252705.png",
        "animal_25274C.png",
        "animal_262705.png",
        "animal_26274C.png",
        "animal_272705.png",
        "animal_27274C.png",
        "animal_282705.png",
        "animal_28274C.png",
        "animal_292705.png",
        "animal_29274C.png",
        "animal_302705.png",
        "animal_30274C.png",
        "animal_312705.png",
        "animal_31274C.png",
        "animal_322705.png",
        "animal_32274C.png",
        "animal_32705.png",
        "animal_3274C.png",
        "animal_332705.png",
        "animal_33274C.png",
        "animal_342705.png",
        "animal_34274C.png",
        "animal_352705.png",
        "animal_35274C.png",
        "animal_362705.png",
        "animal_36274C.png",
        "animal_372705.png",
        "animal_37274C.png",
        "animal_382705.png",
        "animal_38274C.png",
        "animal_392705.png",
        "animal_39274C.png",
        "animal_402705.png",
        "animal_40274C.png",
        "animal_412705.png",
        "animal_41274C.png",
        "animal_422705.png",
        "animal_42274C.png",
        "animal_42705.png",
        "animal_4274C.png",
        "animal_432705.png",
        "animal_43274C.png",
        "animal_442705.png",
        "animal_44274C.png",
        "animal_452705.png",
        "animal_45274C.png",
        "animal_462705.png",
        "animal_46274C.png",
        "animal_472705.png",
        "animal_47274C.png",
        "animal_482705.png",
        "animal_48274C.png",
        "animal_492705.png",
        "animal_49274C.png",
        "animal_502705.png",
        "animal_50274C.png",
        "animal_512705.png",
        "animal_51274C.png",
        "animal_522705.png",
        "animal_52274C.png",
        "animal_52705.png",
        "animal_5274C.png",
        "animal_532705.png",
        "animal_53274C.png",
        "animal_542705.png",
        "animal_54274C.png",
        "animal_552705.png",
        "animal_55274C.png",
        "animal_562705.png",
        "animal_56274C.png",
        "animal_572705.png",
        "animal_57274C.png",
        "animal_582705.png",
        "animal_58274C.png",
        "animal_592705.png",
        "animal_59274C.png",
        "animal_602705.png",
        "animal_60274C.png",
        "animal_612705.png",
        "animal_61274C.png",
        "animal_622705.png",
        "animal_62274C.png",
        "animal_62705.png",
        "animal_6274C.png",
        "animal_632705.png",
        "animal_63274C.png",
        "animal_642705.png",
        "animal_64274C.png",
        "animal_652705.png",
        "animal_65274C.png",
        "animal_662705.png",
        "animal_66274C.png",
        "animal_672705.png",
        "animal_67274C.png",
        "animal_682705.png",
        "animal_68274C.png",
        "animal_692705.png",
        "animal_69274C.png",
        "animal_702705.png",
        "animal_70274C.png",
        "animal_712705.png",
        "animal_71274C.png",
        "animal_722705.png",
        "animal_72274C.png",
        "animal_72705.png",
        "animal_7274C.png",
        "animal_732705.png",
        "animal_73274C.png",
        "animal_742705.png",
        "animal_74274C.png",
        "animal_752705.png",
        "animal_75274C.png",
        "animal_762705.png",
        "animal_76274C.png",
        "animal_772705.png",
        "animal_77274C.png",
        "animal_782705.png",
        "animal_78274C.png",
        "animal_792705.png",
        "animal_79274C.png",
        "animal_802705.png",
        "animal_80274C.png",
        "animal_812705.png",
        "animal_81274C.png",
        "animal_822705.png",
        "animal_82274C.png",
        "animal_82705.png",
        "animal_8274C.png",
        "animal_832705.png",
        "animal_83274C.png",
        "animal_842705.png",
        "animal_84274C.png",
        "animal_852705.png",
        "animal_85274C.png",
        "animal_862705.png",
        "animal_86274C.png",
        "animal_872705.png",
        "animal_87274C.png",
        "animal_882705.png",
        "animal_88274C.png",
        "animal_892705.png",
        "animal_89274C.png",
        "animal_902705.png",
        "animal_90274C.png",
        "animal_912705.png",
        "animal_91274C.png",
        "animal_922705.png",
        "animal_92274C.png",
        "animal_92705.png",
        "animal_9274C.png",
        "animal_932705.png",
        "animal_93274C.png",
        "animal_942705.png",
        "animal_94274C.png",
        "animal_952705.png",
        "animal_95274C.png",
        "animal_962705.png",
        "animal_96274C.png",
        "animal_972705.png",
        "animal_97274C.png",
        "animal_982705.png",
        "animal_98274C.png",
        "animal_992705.png",
        "animal_99274C.png",
        "art_02705.png",
        "art_0274C.png",
        "art_1002705.png",
        "art_1012705.png",
        "art_1022705.png",
        "art_102705.png",
        "art_10274C.png",
        "art_1032705.png",
        "art_1042705.png",
        "art_1052705.png",
        "art_1062705.png",
        "art_1072705.png",
        "art_1082705.png",
        "art_1092705.png",
        "art_1102705.png",
        "art_1112705.png",
        "art_1122705.png",
        "art_112705.png",
        "art_11274C.png",
        "art_1132705.png",
        "art_1142705.png",
        "art_1152705.png",
        "art_1162705.png",
        "art_1172705.png",
        "art_1182705.png",
        "art_1192705.png",
        "art_122705.png",
        "art_12274C.png",
        "art_12705.png",
        "art_1274C.png",
        "art_132705.png",
        "art_13274C.png",
        "art_142705.png",
        "art_14274C.png",
        "art_152705.png",
        "art_15274C.png",
        "art_162705.png",
        "art_16274C.png",
        "art_172705.png",
        "art_17274C.png",
        "art_182705.png",
        "art_18274C.png",
        "art_192705.png",
        "art_19274C.png",
        "art_202705.png",
        "art_20274C.png",
        "art_212705.png",
        "art_21274C.png",
        "art_222705.png",
        "art_22274C.png",
        "art_22705.png",
        "art_2274C.png",
        "art_232705.png",
        "art_23274C.png",
        "art_242705.png",
        "art_24274C.png",
        "art_252705.png",
        "art_25274C.png",
        "art_262705.png",
        "art_26274C.png",
        "art_272705.png",
        "art_27274C.png",
        "art_282705.png",
        "art_28274C.png",
        "art_292705.png",
        "art_29274C.png",
        "art_302705.png",
        "art_30274C.png",
        "art_312705.png",
        "art_31274C.png",
        "art_322705.png",
        "art_32274C.png",
        "art_32705.png",
        "art_3274C.png",
        "art_332705.png",
        "art_33274C.png",
        "art_342705.png",
        "art_34274C.png",
        "art_352705.png",
        "art_35274C.png",
        "art_362705.png",
        "art_36274C.png",
        "art_372705.png",
        "art_37274C.png",
        "art_382705.png",
        "art_38274C.png",
        "art_392705.png",
        "art_39274C.png",
        "art_402705.png",
        "art_40274C.png",
        "art_412705.png",
        "art_41274C.png",
        "art_422705.png",
        "art_42274C.png",
        "art_42705.png",
        "art_4274C.png",
        "art_432705.png",
        "art_43274C.png",
        "art_442705.png",
        "art_44274C.png",
        "art_452705.png",
        "art_45274C.png",
        "art_462705.png",
        "art_46274C.png",
        "art_472705.png",
        "art_47274C.png",
        "art_482705.png",
        "art_48274C.png",
        "art_492705.png",
        "art_49274C.png",
        "art_502705.png",
        "art_50274C.png",
        "art_512705.png",
        "art_51274C.png",
        "art_522705.png",
        "art_52274C.png",
        "art_52705.png",
        "art_5274C.png",
        "art_532705.png",
        "art_53274C.png",
        "art_542705.png",
        "art_54274C.png",
        "art_552705.png",
        "art_55274C.png",
        "art_562705.png",
        "art_56274C.png",
        "art_572705.png",
        "art_57274C.png",
        "art_582705.png",
        "art_58274C.png",
        "art_592705.png",
        "art_59274C.png",
        "art_602705.png",
        "art_60274C.png",
        "art_612705.png",
        "art_61274C.png",
        "art_622705.png",
        "art_62274C.png",
        "art_62705.png",
        "art_6274C.png",
        "art_632705.png",
        "art_63274C.png",
        "art_642705.png",
        "art_64274C.png",
        "art_652705.png",
        "art_65274C.png",
        "art_662705.png",
        "art_66274C.png",
        "art_672705.png",
        "art_67274C.png",
        "art_682705.png",
        "art_68274C.png",
        "art_692705.png",
        "art_69274C.png",
        "art_702705.png",
        "art_70274C.png",
        "art_712705.png",
        "art_71274C.png",
        "art_722705.png",
        "art_72274C.png",
        "art_72705.png",
        "art_7274C.png",
        "art_732705.png",
        "art_73274C.png",
        "art_742705.png",
        "art_74274C.png",
        "art_752705.png",
        "art_75274C.png",
        "art_762705.png",
        "art_76274C.png",
        "art_772705.png",
        "art_77274C.png",
        "art_782705.png",
        "art_78274C.png",
        "art_792705.png",
        "art_79274C.png",
        "art_802705.png",
        "art_80274C.png",
        "art_812705.png",
        "art_81274C.png",
        "art_822705.png",
        "art_82274C.png",
        "art_82705.png",
        "art_8274C.png",
        "art_832705.png",
        "art_83274C.png",
        "art_842705.png",
        "art_84274C.png",
        "art_852705.png",
        "art_85274C.png",
        "art_862705.png",
        "art_86274C.png",
        "art_872705.png",
        "art_87274C.png",
        "art_882705.png",
        "art_88274C.png",
        "art_892705.png",
        "art_89274C.png",
        "art_902705.png",
        "art_90274C.png",
        "art_912705.png",
        "art_91274C.png",
        "art_922705.png",
        "art_92274C.png",
        "art_92705.png",
        "art_9274C.png",
        "art_932705.png",
        "art_93274C.png",
        "art_942705.png",
        "art_94274C.png",
        "art_952705.png",
        "art_95274C.png",
        "art_962705.png",
        "art_96274C.png",
        "art_972705.png",
        "art_97274C.png",
        "art_982705.png",
        "art_98274C.png",
        "art_992705.png",
        "art_99274C.png",
        "photog_02705.png",
        "photog_0274C.png",
        "photog_1002705.png",
        "photog_100274C.png",
        "photog_1012705.png",
        "photog_101274C.png",
        "photog_1022705.png",
        "photog_102274C.png",
        "photog_102705.png",
        "photog_10274C.png",
        "photog_1032705.png",
        "photog_103274C.png",
        "photog_1042705.png",
        "photog_104274C.png",
        "photog_1052705.png",
        "photog_105274C.png",
        "photog_1062705.png",
        "photog_106274C.png",
        "photog_1072705.png",
        "photog_107274C.png",
        "photog_1082705.png",
        "photog_108274C.png",
        "photog_1092705.png",
        "photog_109274C.png",
        "photog_1102705.png",
        "photog_110274C.png",
        "photog_1112705.png",
        "photog_111274C.png",
        "photog_1122705.png",
        "photog_112274C.png",
        "photog_112705.png",
        "photog_11274C.png",
        "photog_1132705.png",
        "photog_113274C.png",
        "photog_1142705.png",
        "photog_114274C.png",
        "photog_115274C.png",
        "photog_116274C.png",
        "photog_117274C.png",
        "photog_118274C.png",
        "photog_119274C.png",
        "photog_122705.png",
        "photog_12274C.png",
        "photog_12705.png",
        "photog_1274C.png",
        "photog_132705.png",
        "photog_13274C.png",
        "photog_142705.png",
        "photog_14274C.png",
        "photog_152705.png",
        "photog_15274C.png",
        "photog_162705.png",
        "photog_16274C.png",
        "photog_172705.png",
        "photog_17274C.png",
        "photog_182705.png",
        "photog_18274C.png",
        "photog_192705.png",
        "photog_19274C.png",
        "photog_202705.png",
        "photog_20274C.png",
        "photog_212705.png",
        "photog_21274C.png",
        "photog_222705.png",
        "photog_22274C.png",
        "photog_22705.png",
        "photog_2274C.png",
        "photog_232705.png",
        "photog_23274C.png",
        "photog_242705.png",
        "photog_24274C.png",
        "photog_252705.png",
        "photog_25274C.png",
        "photog_262705.png",
        "photog_26274C.png",
        "photog_272705.png",
        "photog_27274C.png",
        "photog_282705.png",
        "photog_28274C.png",
        "photog_292705.png",
        "photog_29274C.png",
        "photog_302705.png",
        "photog_30274C.png",
        "photog_312705.png",
        "photog_31274C.png",
        "photog_322705.png",
        "photog_32274C.png",
        "photog_32705.png",
        "photog_3274C.png",
        "photog_332705.png",
        "photog_33274C.png",
        "photog_342705.png",
        "photog_34274C.png",
        "photog_352705.png",
        "photog_35274C.png",
        "photog_362705.png",
        "photog_36274C.png",
        "photog_372705.png",
        "photog_37274C.png",
        "photog_382705.png",
        "photog_38274C.png",
        "photog_392705.png",
        "photog_39274C.png",
        "photog_402705.png",
        "photog_40274C.png",
        "photog_412705.png",
        "photog_41274C.png",
        "photog_422705.png",
        "photog_42274C.png",
        "photog_42705.png",
        "photog_4274C.png",
        "photog_432705.png",
        "photog_43274C.png",
        "photog_442705.png",
        "photog_44274C.png",
        "photog_452705.png",
        "photog_45274C.png",
        "photog_462705.png",
        "photog_46274C.png",
        "photog_472705.png",
        "photog_47274C.png",
        "photog_482705.png",
        "photog_48274C.png",
        "photog_492705.png",
        "photog_49274C.png",
        "photog_502705.png",
        "photog_50274C.png",
        "photog_512705.png",
        "photog_51274C.png",
        "photog_522705.png",
        "photog_52274C.png",
        "photog_52705.png",
        "photog_5274C.png",
        "photog_532705.png",
        "photog_53274C.png",
        "photog_542705.png",
        "photog_54274C.png",
        "photog_552705.png",
        "photog_55274C.png",
        "photog_562705.png",
        "photog_56274C.png",
        "photog_572705.png",
        "photog_57274C.png",
        "photog_582705.png",
        "photog_58274C.png",
        "photog_592705.png",
        "photog_59274C.png",
        "photog_602705.png",
        "photog_60274C.png",
        "photog_612705.png",
        "photog_61274C.png",
        "photog_622705.png",
        "photog_62274C.png",
        "photog_62705.png",
        "photog_6274C.png",
        "photog_632705.png",
        "photog_63274C.png",
        "photog_642705.png",
        "photog_64274C.png",
        "photog_652705.png",
        "photog_65274C.png",
        "photog_662705.png",
        "photog_66274C.png",
        "photog_672705.png",
        "photog_67274C.png",
        "photog_682705.png",
        "photog_68274C.png",
        "photog_692705.png",
        "photog_69274C.png",
        "photog_702705.png",
        "photog_70274C.png",
        "photog_712705.png",
        "photog_71274C.png",
        "photog_722705.png",
        "photog_72274C.png",
        "photog_72705.png",
        "photog_7274C.png",
        "photog_732705.png",
        "photog_73274C.png",
        "photog_742705.png",
        "photog_74274C.png",
        "photog_752705.png",
        "photog_75274C.png",
        "photog_762705.png",
        "photog_76274C.png",
        "photog_772705.png",
        "photog_77274C.png",
        "photog_782705.png",
        "photog_78274C.png",
        "photog_792705.png",
        "photog_79274C.png",
        "photog_802705.png",
        "photog_80274C.png",
        "photog_812705.png",
        "photog_81274C.png",
        "photog_822705.png",
        "photog_82274C.png",
        "photog_82705.png",
        "photog_8274C.png",
        "photog_832705.png",
        "photog_83274C.png",
        "photog_842705.png",
        "photog_84274C.png",
        "photog_852705.png",
        "photog_85274C.png",
        "photog_862705.png",
        "photog_86274C.png",
        "photog_872705.png",
        "photog_87274C.png",
        "photog_882705.png",
        "photog_88274C.png",
        "photog_892705.png",
        "photog_89274C.png",
        "photog_902705.png",
        "photog_90274C.png",
        "photog_912705.png",
        "photog_91274C.png",
        "photog_922705.png",
        "photog_92274C.png",
        "photog_92705.png",
        "photog_9274C.png",
        "photog_932705.png",
        "photog_93274C.png",
        "photog_942705.png",
        "photog_94274C.png",
        "photog_952705.png",
        "photog_95274C.png",
        "photog_962705.png",
        "photog_96274C.png",
        "photog_972705.png",
        "photog_97274C.png",
        "photog_982705.png",
        "photog_98274C.png",
        "photog_992705.png",
        "photog_99274C.png"
    };

    private readonly Random random;
    private readonly List<string> aiImages = new();
    private readonly List<string> realImages = new();

    public ImageFactory(Random random)
    {
        this.random = random;

        foreach (var image in images)
        {
            if (image[^5] == 'C')
            {
                aiImages.Add(image);
            }
            else
            {
                realImages.Add(image);
            }
        }

        Console.WriteLine($"Ai images: {aiImages.Count}");
        Console.WriteLine($"Real images: {realImages.Count}");
    }

    private string GetRandomImages(List<string> images)
    {
        int index = random.Next(0, images.Count);
        return images[index];
    }

    private static string GetImageUrl(string image, bool isHighRes = true) => $"https://raw.githubusercontent.com/Metater/ISpyAi/main/images/{(isHighRes ? "512" : "256")}/{image}";

    public string GetRandomAiImageUrl(bool isHighRes) => GetImageUrl(GetRandomImages(aiImages), isHighRes);
    public string GetRandomRealImageUrl(bool isHighRes) => GetImageUrl(GetRandomImages(realImages), isHighRes);
}