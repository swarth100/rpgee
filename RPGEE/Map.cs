﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    public class Map
    {
        public enum Status
        {
            Move,
            Draw,
            Inspect,
            Delete
        }

        /* Public fields */

        /* Edit status for the map */
        public Status status { get; set; }
        public DraggablePictureBox PictureBox { get; set; }

        #region blockIds

        /** All of the following arrays are courtesy of Capasha from EEditor's source code */

        private static int[,] blockInit = new int[,] {
                { 0, 0 },{ 1, 1 },{ 2, 2 },{ 3, 3 },{ 4, 4 },{ 5, 5 },{ 6, 6 },{ 7, 7 },{ 8, 8 },{ 9, 9 },{ 10, 10 },
                { 11, 11 },{ 12, 12 },{ 13, 13 },{ 14, 14 },{ 15, 15 },{ 16, 16 },{ 17, 17 },{ 18, 18 },{ 19, 19 },
                { 20, 20 },{ 21, 21 },{ 22, 22 },{ 23, 23 },{ 24, 24 },{ 25, 25 },{ 26, 26 },{ 27, 27 },{ 28, 28 },
                { 29, 29 },{ 30, 30 },{ 31, 31 },{ 32, 32 },{ 33, 33 },{ 34, 34 },{ 35, 35 },{ 36, 36 },{ 37, 37 },
                { 38, 38 },{ 39, 39 },{ 40, 40 },{ 41, 41 },{ 42, 42 },{ 43, 43 },{ 44, 44 },{ 45, 45 },{ 46, 46 },
                { 47, 47 },{ 48, 48 },{ 49, 49 },{ 51, 51 },{ 52, 52 },{ 53, 53 },{ 54, 54 },{ 55, 55 },
                { 56, 56 },{ 57, 57 },{ 58, 58 },{ 59, 59 },{ 60, 60 },{ 61, 61 },{ 62, 62 },{ 63, 63 },{ 64, 64 },
                { 65, 65 },{ 66, 66 },{ 67, 67 },{ 68, 68 },{ 69, 69 },{ 70, 70 },{ 71, 71 },{ 72, 72 },{ 73, 73 },
                { 74, 74 },{ 75, 75 },{ 76, 76 },{ 77, 77 },{ 78, 78 },{ 79, 79 },{ 80, 80 },{ 81, 81 },{ 82, 82 },
                { 83, 83 },{ 84, 84 },{ 85, 85 },{ 86, 86 },{ 87, 87 },{ 88, 88 },{ 89, 89 },{ 90, 90 },{ 91, 91 },
                { 92, 92 },{ 93, 93 },{ 94, 94 },{ 95, 95 },{ 96, 96 },{ 97, 97 },
                { 120, 98}, { 122, 99}, { 123, 100}, { 124, 101}, { 125, 102}, { 126, 103}, { 127, 104}, { 128, 105},
                { 129, 106}, { 130, 107}, { 131, 108}, { 132, 109}, { 133, 110}, { 134, 111}, { 135, 112}, { 137, 114},
                { 138, 115}, { 139, 116}, { 140, 117}, { 141, 118}, { 142, 119}, { 143, 120}, { 144, 121}, { 145, 122},
                { 146, 123}, { 147, 124}, { 148, 125}, { 149, 126}, { 150, 127}, { 151, 128}, { 152, 129}, { 153, 130},
                { 154, 131}, { 158, 132}, { 159, 133}, { 160, 134}, { 118, 135}, { 162, 136}, { 163, 137}, { 165, 139},
                { 166, 140}, { 167, 141}, { 168, 142}, { 169, 143}, { 170, 144}, { 171, 145}, { 172, 146}, { 173, 147},
                { 174, 148}, { 175, 149}, { 176, 150}, { 177, 151}, { 178, 152}, { 179, 153}, { 180, 154}, { 181, 155},
                { 182, 156}, { 114, 157}, { 115, 158}, { 116, 159}, { 117, 160}, { 186, 161}, { 187, 162}, { 188, 163},
                { 189, 164}, { 190, 165}, { 191, 166}, { 192, 167}, { 193, 168}, { 194, 169}, { 195, 170}, { 196, 171},
                { 197, 172}, { 198, 173}, { 98, 174}, { 99, 175}, { 199, 176}, { 202, 177}, { 203, 178}, { 204, 179},
                { 208, 180}, { 209, 181}, { 210, 182}, { 211, 183}, { 212, 184}, { 213, 185 }, { 214, 186}, {215, 187}, {216, 188},
                {408, 189}, {409, 190}, {410, 191}, {1005, 192}, {1006, 193}, {1007, 194}, {1008, 195}, {1009, 196}, {1010, 197},
                {1012, 198}, {1011, 199}, {1013, 200}, {1014, 201}, {1015, 202}, {1016, 203}, {1017, 204},
                {1018, 205}, {1019, 206}, {1020, 207}, {1021, 208}, {1022, 209}, {1023, 210}, {1024, 211}, {1025, 212}, {1026, 213}, {1029, 214},
                {1030, 215}, {1031, 216}, {1032, 217}, {1033, 218},  {1034, 219}, { 1044, 226 }, { 1045, 227 }, { 1046, 228 },
                { 1035, 220 }, { 1036, 221 }, { 1037, 222 }, { 1038, 223 }, { 1039, 224 }, { 1040, 225 }, { 1047, 229 }, { 1048, 230 }, { 1049, 231 }, { 1050, 232 },
                { 1051, 233 }, { 1057, 234 }, { 1058, 235 }, { 1054, 236 }, { 1055, 237 }, { 1056, 238 }, { 1057, 239 }, { 1058, 240 },
                { 1059, 237 }, { 1060, 238 }, { 1061, 239 }, { 1062, 240 }, { 1063, 241 }, { 459, 233 }, { 1051, 234 }, { 1057, 235 }, { 1058, 236 },
                { 1065, 242 }, { 1066, 243 }, { 1067, 244 }, { 1068, 245 }, { 1069, 246 }, { 1070, 247 }, { 1071, 248 }, { 1072, 249 }, { 1073, 250 }, { 1074, 251 },
                { 472, 252 }, { 1081, 253 }, { 1082, 254}, { 1083, 255}, { 1084, 256}, { 1085, 257}, { 1086, 258}, { 1087, 259},
                { 1088, 260 }, { 1089, 261 }, { 1090, 262 }, { 1091, 263 }, { 1093, 264 }, { 1096, 265 }, { 1097, 266 }, { 1098, 267 }, { 1099, 268 }, { 1100, 269 },
                { 1101, 270 },{ 1102, 271 },{ 1103, 272 },{ 1104, 273 },{ 1105, 274 },{ 1106, 275 },{ 1107, 276 },{ 1108, 277 },{ 1109, 278 },{ 1110, 279 },
                { 1111, 280 },{ 1112, 281 },{ 1113, 282 },{ 1114, 283 },{ 1115, 284 }, { 1518, 285 }, { 1520, 286 }
            };

        private static int[,] miscInit = new int[,] {

                { 119, 0 },{ 300, 1 },{ 337, 2 },{ 113, 3 },{ 185, 4 },{ 184, 5 },{ 157, 6 },{ 156, 7 },{ 121, 8 },{ 50, 9 },{ 243, 10 }, { 136, 16 },
                { 201, 12 },{ 200, 13 },{ 361, 24 }, { 360, 27 }, { 368, 28 }, { 369, 29 }, { 370, 30 }, { 207, 31 }, { 206, 32 }, { 397, 53 }, { 411, 70 }, { 412, 71 },  { 413, 72 }, { 414, 73 }, { 416, 107 },
                { 100, 174 }, { 101, 175 }, { 1001, 55 }, { 1002, 63}, { 1003, 59 }, { 1004, 67 }, { 417, 74}, { 418, 75}, {419, 76}, { 420, 77}, { 421, 78}, { 422, 79 }, { 423, 80 }, { 1028, 100}, { 1027, 93 },
                { 374, 33}, { 381, 112 }, { 242, 108 }, { 385, 255}, { 241, 221 }, { 453, 176 }, { 375, 35 }, { 376, 39 }, { 377, 41 }, { 378, 45 }, { 379, 47 } , { 380, 51 }, { 438, 161 }, { 439, 167 },
                { 300, 1}, { 440, 169 }, { 275 , 117 }, { 329 , 129 }, { 273 , 125 }, { 328 , 157 }, { 327 , 121 },
                { 338,119 }, {339,120 }, { 327,121 }, { 370,30 }, { 456, 215 }, { 457,217 }, { 458,219 }, { 338,137}, { 339,133}, { 340 ,153 },
                { 370, 30}, {1041, 203}, {1042, 207}, {1043, 211 }, { 456, 215} ,{ 457, 217},{ 458, 219}, {447, 179},{ 448, 183},{449, 187}, {450, 191},{451, 195},{ 452, 199}, { 464, 201 }, { 465, 202 }, { 460, 222 },
                { 461, 252}, { 1064, 251}, {1052, 224 }, {1053, 228 }, { 1054 ,232 }, { 1055, 236 }, { 1056, 240 }, { 464, 244 }, { 465, 248 },
                { 467, 259} , {1080, 261}, { 1079, 262 }, { 1081, 263 }, { 1075, 264 }, { 1076, 268}, { 1077, 272}, { 1078, 276 }, { 471, 279 },
                { 475, 283 }, { 476, 286 }, { 477, 289 }, { 481, 292}, { 482, 298 }, { 483, 304}, { 1092, 308 },
                { 497, 311 }, { 498, 317 },
                { 492, 319 }, { 493, 323 }, { 494, 327 }, { 499, 331 }, { 1500, 335 }, { 1502, 338 },
                { 1094, 341 }, { 1095, 340 }, { 1506, 348 }, { 1507, 343 }, { 1510, 352 },
                { 1517, 355 }, { 1519, 360 }, { 1116, 362 }, { 1117, 366 }, { 1118, 370 }, { 1119, 374 },
                { 1120, 378 },{ 1121, 382 },{ 1122, 386 },{ 1123, 390 },{ 1124, 394 },{ 1125, 398 },
            };

        private static int[,] decorInit = new int[,] {
                { 255, 38 }, {424, 177}, { 249, 32 }, { 250, 33}, { 251, 34}, { 252, 35}, { 253, 36}, { 254, 37},
                { 244, 27 }, { 245, 28 }, { 246, 29 }, { 247, 30 }, { 248, 31 }, { 223, 6 },
                { 233, 16 }, { 234, 17 }, { 235, 18 }, { 236, 19 }, { 237, 20 }, { 238, 21 }, { 239, 22 }, { 240, 23 },
                { 256, 39 }, { 257, 40 }, { 258, 41 }, { 259, 42 }, { 260, 43 }, { 227, 10 }, { 431, 184 },  { 432, 185 }, { 433, 186 }, { 434, 187 },
                { 228 , 11 }, { 229 , 12 }, { 230 , 13 }, { 231 , 14 }, { 232 , 15 }, {  224, 7 }, {  225, 8 }, {  226, 9 }, { 218, 1 }, { 219, 2 }, { 220, 3 }, { 221, 4 }, { 222, 5 },
                { 261, 44 }, { 262, 45}, { 263, 46}, { 264, 47}, { 265, 48}, { 266, 49}, { 267, 50},  { 268, 51}, { 269, 52}, { 270, 53},
                { 271, 54}, { 272, 55}, { 435, 188}, { 436, 189}, { 276, 59 }, { 277, 60 }, { 278, 61 },  { 279, 62 }, { 280, 63 }, { 281, 64 }, { 282, 65 },  { 283, 66 },  { 284, 67 },
                { 285, 68},{ 286, 69 },{ 287, 70 },{ 288, 71 },{ 289, 72 }, { 290, 73 },{ 291, 74 },{ 292, 75 },{ 293, 76 },{ 294, 77 },{ 295, 78 }, { 296, 79 },{ 297, 80 },{ 298, 81 },{ 299, 82 },
                { 301, 83 },{ 302, 84 },{ 303, 85 },{ 304, 86 },{ 305, 87 },{ 306, 88 },{307, 89},{308, 90},{309, 91},{310, 92},
                { 311, 93 },{ 312, 94 },{ 313, 95 },{ 314, 96 },{315,  97 },{ 316, 98 },{ 317, 99 },{ 318,  100}, { 319, 101 },{ 320, 102 },{ 321, 103 },{ 322, 104 },{ 323, 105 },{ 324, 106 },
                {325, 107}, { 326, 108}, { 437, 190 }, { 330, 112 }, { 332, 114 }, { 333, 115 }, { 334, 116 }, { 335, 117 }, {428, 181 }, { 429, 182 }, { 430, 183 }, { 331, 113 },
                { 336, 118}, { 425,178 }, {426, 179 }, { 427,180 },
                { 274,57 }, { 341,122 }, { 342,123 },
                { 343,124 }, { 344,125 }, { 345,126 }, { 346, 127 }, { 347, 128 }, { 348, 129 }, { 349, 130 }, { 350, 131 }, { 351,132 },
                { 352, 133}, { 353, 134}, { 354, 135}, { 355, 136}, { 356, 137},
                { 357, 138},{ 358, 139},{ 359, 140},
                { 362, 141},{ 363, 142},{ 364, 143},{ 365, 144},{ 366, 145},{ 367, 146},
                { 398,165 },{ 399,166 },{ 400,167 },{ 401,168 },{ 402,169 },{ 403,170 },{ 404,171 },
                { 405,172 },{ 406,173 },{ 407,174 },
                { 415,175 },
                { 371, 147}, { 372, 148}, { 373, 149 },
                {382, 150}, {383, 151}, {384, 152},
                {386, 154}, {387, 155}, {388, 156}, {389, 157},
                {390, 158}, {391, 159}, {392, 160}, {393, 161}, {394, 162}, {395, 163}, {396, 164},
                {441, 191}, {442, 192}, {443, 193}, {444, 194}, {445, 195},
                { 446,196 },
                {454, 197}, {455, 198},
                { 466, 199 }, { 462, 200 }, { 463, 201 }, { 468, 202 }, { 469, 203}, { 470 , 204 },
                { 473, 205 }, { 474, 206 }, { 478, 209 }, { 479, 208 }, { 480, 207 }, { 495, 218 }, { 496, 219 },
                { 487, 213 }, { 488, 214 },{ 489, 215 },{ 490, 216 },{ 491, 217 }, { 1501, 220 },
                { 484, 212 }, { 485, 211 }, { 486, 210 }, { 1503,221  }, { 1504,222  }, { 1505,223  },
                { 1508,224 }, { 1509, 225 }, { 1511, 226 }, { 1512, 227 }, { 1513, 228 }, { 1514, 229 }, { 1515, 230 },
                { 1516, 231 }
            };

        private static int[,] bgInit = new int[,] {
                { 500,0}, { 501,1}, { 502,2}, { 503,3}, { 504,4}, { 505,5}, { 506,6}, { 507,7}, { 508,8}, { 509,9}, { 510,10},
                { 511,11}, { 512,12}, { 513,13}, { 514,14}, { 515,15}, { 516,16}, { 517,17}, { 518,18}, { 519,19}, { 520,20},
                { 521,21}, { 522,22}, { 523,23}, { 524,24}, { 525,25}, { 526,26}, { 527,27}, { 528,28}, { 529,29}, { 530,30},
                { 531,31}, { 532,32}, { 533,33}, { 534,34}, { 535,35}, { 536,36}, { 537,37}, { 538,38}, { 539,39}, { 540,40},
                { 541,41}, { 542,42}, { 543,43}, { 544,44}, { 545,45}, { 546,46}, { 547,47}, { 548,48}, { 549,49}, { 550,50},
                { 551,51}, { 552,52}, { 553,53}, { 554,54}, { 555,55}, { 556,56}, { 557,57}, { 558,58}, { 559,59}, { 560,60},
                { 561,61}, { 562,62}, { 563,63}, { 564,64}, { 565,65}, { 566,66}, { 567,67}, { 568,68}, { 569,69}, { 570,70},
                { 571,71}, { 572,72}, { 573,73}, { 574,74}, { 575,75}, { 576,76}, { 577,77}, { 578,78}, { 579,79}, { 580,80},
                { 581,81}, { 582,82}, { 583,83}, { 584,84}, { 585,85}, { 586,86}, { 587,87}, { 588,88}, { 589,89}, { 590,90},
                { 591,91}, { 592,92}, { 593,93}, { 594,94}, { 595,95}, { 596,96}, { 597,97}, { 598,98}, { 599,99}, { 600,100},
                { 601,101}, { 602,102}, { 603,103}, { 604,104}, { 605,105}, { 606,106}, { 607,107}, { 608,108}, { 609,109}, { 610,110},
                { 611,111}, { 612,112}, { 613,113}, { 614,114}, { 615,115}, { 616,116}, { 617,117}, { 618,118}, { 619,119}, { 620,120},
                { 621,121}, { 622,122}, { 623,123}, { 624,124}, { 625,125}, { 626,126}, { 627,127}, { 628,128}, { 629,129}, { 630,130},
                { 637,131}, { 638,132}, { 639,133}, { 640,134}, { 641,135}, { 642,136}, { 643,137}, { 644,138}, { 645,139}, { 646,140},
                { 647,141}, { 648,142}, { 649,143}, { 650,144}, { 651,145}, { 652,146}, { 653,147}, { 654,148}, { 655,149}, { 656,150},
                { 657,151}, { 658,152}, { 659,153}, { 660,154}, { 661,155}, { 662,156}, { 663,157}, { 664,158}, { 665,159}, { 666,160},
                { 667,161}, { 668,162}, { 669,163}, { 670,164}, { 671,165}, { 672,166}, { 673,167}, { 674,168}, { 675,169}, { 676,170},
                { 677, 171 },{ 539, 39 }, { 540,40 },{ 637,131 },{ 550,50 }, { 551,51 }, { 552,52 }, { 553,53 },
                { 554,54 }, { 555,55 }, { 559,59 }, { 560,60 },{ 561,61 }, { 562,62 }, { 688,182 }, { 689,183 }, { 690,184 }, { 691,185 }, { 692,186 }, { 693,187 },
                { 564,64 }, { 565,65 }, { 566,66 }, { 567,67 }, { 667,161 }, { 668,162 }, { 669,163 }, { 670,164 },
                { 568,68 }, { 569,69 }, { 570,70 }, { 571,71 }, { 572,72 }, { 573,73 },{ 574,74 }, { 575,75 }, { 576,76 }, { 577,77 }, { 578,78 },
                { 579,79 }, { 580,80 }, { 581,81 }, { 582,82 }, { 583,83 }, { 584,84 },{ 585,85 }, { 586,86 }, { 587,87 }, { 588,88 }, { 589,89 },
                { 594,94 }, { 595,95 }, { 596,96 }, { 597,97 }, { 598,98 },{ 599,99 }, { 600,100 }, { 590,90 }, { 591,91 }, { 592,92 }, { 556,56 }, { 593,93 },
                { 601,101 }, { 602,102 }, { 603,103 }, { 604,104 },{ 605, 105 }, { 673, 167 }, { 674, 168 }, { 675, 169 },
                { 608,108 }, { 609,109 }, { 663,157 }, { 664,158 }, { 665,159 }, { 666,160 },
                { 617,117 }, { 618,118 }, { 619,119 }, { 620,120 }, { 621,121 }, { 622,122 }, { 623,123 },
                { 624,124 }, { 625,125 }, { 626,126 },{ 627,127 }, { 628,128 }, { 629,129 },
                { 557,57 }, { 630,130 },{ 638,132 }, { 639,133 }, { 640,134 },{ 641,135 }, { 642,136 }, { 643,137 },
                { 655,149 }, { 656,150 }, { 657,151 }, { 658,152 }, { 659,153 }, { 660,154 }, { 661,155 }, { 662,156 }, { 663,157 }, { 664,158 }, { 665,159 }, { 666,160 },
                { 678,172 }, { 679,173 }, { 680,174 }, { 681,175 }, { 682,176 }, { 683, 177 }, { 684, 178 }, {685, 179}, {686, 180}, { 687, 181 },
                { 694,188 }, { 695,189 }, { 696,190 }, { 697, 191 }, { 698, 192}, { 699, 193}, { 700, 194 }, { 701, 195 }, { 702, 196 }, { 703, 197 }, { 709, 198 }, { 710, 199 }, { 711, 200 },
                { 704, 201 }, { 705, 202 }, { 706, 203 }, { 707, 204 }, { 708, 205 }, { 712, 206 }, { 713, 207 }, { 714, 208 },
                 { 715, 209 },{ 716, 210 },{ 717, 211 },{ 718, 212 },{ 719, 213 }, { 720, 219 },
                 { 721, 214 }, { 722, 215 },{ 723, 216 },{ 724, 217 },{ 725, 218 }, { 726, 220 }, { 727, 221 },
                 { 728, 222 },{ 729, 223 },{ 730, 224 },{ 731, 225 },{ 732, 226 },{ 733, 227 },{ 734, 228 },
                 { 735, 229 },{ 736, 230 },{ 737, 231 },{ 738, 232 },{ 739, 233 },{ 740, 234 },{ 741, 235 },{ 742, 236 },
            };

        #endregion

        /* Private fields */
        public static int blockSize = 16;
        private static int[] backgroundRef = new int[2000];
        private static int[] foregroundRef = new int[2000];
        private readonly ToolTip inspectTt;

        /* Image resource references */
        private static Image frontImage = Properties.Resources.BLOCKS_front;
        private static Image miscImage = Properties.Resources.BLOCKS_misc;
        private static Image decoImage = Properties.Resources.BLOCKS_deco;
        private static Image backImage = Properties.Resources.BLOCKS_back;

        /* Map instance */
        private Image mapScreen;
        private static Image map;

        /* Zones */
        private List<Zone> Zones = new List<Zone>();
        private int selectedZone;

        /** Initialise a new map instance */
        public Map()
        {
            #region blockInit
            /* Prior to map loading, setup the bitmap lookup arrays */

            /* Initialis backgrounds */
            for (int i = 0; i < bgInit.Length / 2; i++)
            {
                backgroundRef[bgInit[i, 0]] = bgInit[i, 1]; //Add blockid and imageid
            }

            int blockCount = 0;

            /* Initialise main blocks */
            for (int i = 0; i < blockInit.Length / 2; i++)
            {
                foregroundRef[blockInit[i, 0]] = blockInit[i, 1]; //Add blockid and imageid
                blockCount++;
            }

            /* Magic number fix to allow for morphable blocks */
            blockCount -= 5;
            int miscCount = 0;

            /* Initialise miscellaneous */
            for (int i = 0; i < miscInit.Length / 2; i++)
            {
                foregroundRef[miscInit[i, 0]] = miscInit[i, 1] + blockCount; //Add blockid and imageid
                miscCount++;
            }

            /* Magic number fix to allow for morphable blocks */
            miscCount += 262;
            blockCount += miscCount;

            /* Initialise decorations */
            for (int i = 0; i < decorInit.Length / 2; i++)
            {
                foregroundRef[decorInit[i, 0]] = decorInit[i, 1] + blockCount; //Add blockid and imageid
            }

            #endregion

            inspectTt = new ToolTip();
        }

        public void setPictureBox(DraggablePictureBox img)
        {
            PictureBox = img;
        }

        /** Public method to initialise the map Image
         * The rendered map is also placed on the screen */
        public void loadMap ()
        {
            #region imgInit

            /* Generate a new image */
            PictureBox.Image = new Bitmap(BackgroundThread.width * blockSize, BackgroundThread.height * blockSize);
            PictureBox.Size = new Size(PictureBox.Image.Width, PictureBox.Image.Height);

            /* Save the image's screen */
            mapScreen = PictureBox.Image;

            /* Initialise zones */
            Zone defaultZone = new Zone(PictureBox.Image, Zones);
            changeSelectedZone(defaultZone.getListIndex());

            /* Clone the new image as the map */
            map = new Bitmap(PictureBox.Image.Width, PictureBox.Image.Height);

            /* Initialise status to Move mode */
            status = Status.Move;

            /** Generate image containing all layer 0 blocks
             * The three types of layer 0 blocks, namely those from frontImage, miscImage and decoImage are appended into the
             * same massive Bitmap.
             * They are indexed subsequently, and indexes are assigned and adjusted above */
            Image blockImage = new Bitmap(frontImage.Width + miscImage.Width + decoImage.Width, blockSize);
            using (Graphics blockGraphics = Graphics.FromImage(blockImage))
            {
                /* Regular frontImage blocks can start at (X,Y) = (0,0) */
                Rectangle frontSrc = new Rectangle(0, 0, frontImage.Width, blockSize);
                Rectangle frontDest = new Rectangle(0, 0, frontImage.Width, blockSize);
                blockGraphics.DrawImage(frontImage, frontDest, frontSrc, GraphicsUnit.Pixel);

                /* Miscellaneous blocks start at (X,Y) = (frontImage.Width, 0) */
                Rectangle miscSrc = new Rectangle(0, 0, miscImage.Width, blockSize);
                Rectangle miscDest = new Rectangle(frontImage.Width, 0, miscImage.Width, blockSize);
                blockGraphics.DrawImage(miscImage, miscDest, miscSrc, GraphicsUnit.Pixel);

                /* Decorative blocks start at (X,Y) = (frontImage.Width + miscImage.Width, 0) */
                Rectangle decoSrc = new Rectangle(0, 0, decoImage.Width, blockSize);
                Rectangle decoDest = new Rectangle(frontImage.Width + miscImage.Width, 0, decoImage.Width, blockSize);
                blockGraphics.DrawImage(decoImage, decoDest, decoSrc, GraphicsUnit.Pixel);
            }

            #endregion

            /* Render the mapData onto the map with the appropriate Sprites */
            using (var mapGraphics = Graphics.FromImage(map))
            {
                lock (BackgroundThread._roomDataLock)
                {
                    /* Iterate through all the blocks in the roomData and display them one by one */
                    for (int y = 0; y < BackgroundThread.height; y ++)
                    {
                        for (int x = 0; x < BackgroundThread.width; x++)
                        {
                            /* Create rectangle for displaying the block. It is shared between foreground and background */
                            Rectangle destRect = new Rectangle(x * blockSize, y * blockSize, blockSize, blockSize);

                            /* Initialise and display the background for the given tile. Displays empty backgrounds*/
                            int backgroundBlockID = backgroundRef[BackgroundThread.roomData[1, x, y]];
                            Rectangle backRect = new Rectangle(backgroundBlockID * blockSize, 0, blockSize, blockSize);

                            /* Draw background to screen */
                            mapGraphics.DrawImage(backImage, destRect, backRect, GraphicsUnit.Pixel);

                            /* Initialise and display the foreground for the given tile. Does not display empty blocks */
                            int foregroundBlockID = foregroundRef[BackgroundThread.roomData[0, x, y]];
                            if (foregroundBlockID != 0)
                            {
                                Rectangle frontRect = new Rectangle(foregroundBlockID * blockSize, 0, blockSize, blockSize);

                                /* Draw foreground to screen */
                                mapGraphics.DrawImage(blockImage, destRect, frontRect, GraphicsUnit.Pixel);
                            }
                        }
                    }
                }

                /* Render the map onto the screen */
                renderMap();

                RpgEE.spawnMap();
            }
        }

        public void drawPoint(Point p)
        {
            drawPointHelper(p, true);
        }

        public void erasePoint(Point p)
        {
            drawPointHelper(p, false);
        }

        /** Private function invoked during a Draw Event to the screen. Can both draw or erase.
         * Map must be in Draw/Erase Status and cursor can be dragged */
        private void drawPointHelper(Point p, bool draw)
        {
            /* Round the given cursor point to the nearest grid-alligned blockSize square */
            Point roundP = getRoundPoint(p);

            /* Determine which zone is currently selected for drawing */
            Zone curZone = Zones[selectedZone];

            bool drawnPoint = curZone.isPointSelected(roundP);

            if ((!drawnPoint && draw) || (drawnPoint && !draw))
            {

                /* Render the new point onto the current Zone's overlay */
                using (var overlayGraphics = Graphics.FromImage(curZone.Image))
                {
                    /* Conditionally select the brush to use */
                    if (draw)
                        overlayGraphics.FillRectangle(curZone.Brush, new Rectangle(roundP, new Size(blockSize, blockSize)));
                    else
                        removeBitmapRegion(roundP, curZone);

                    // overlayGraphics.Re
                    // overlayGraphics.FillRectangle(brush, new Rectangle(roundP, new Size(blockSize, blockSize)));
                }

                /* Conditionally draw or erase */
                if (draw)
                    curZone.addPoint(roundP);
                else
                    curZone.removePoint(roundP);

                /* Render all overlays onto the screen */
                renderPoint(roundP);
            }
        }

        /** Public function invoked to spawn a new Zone to be drawn onto the map */
        public void addNewZone()
        {
            changeSelectedZone(new Zone(map, Zones).getListIndex());
        }

        /** Public function invoked by clicking on a Zone's Name Label */
        public void changeSelectedZone(int newZone)
        {
            Console.WriteLine(selectedZone);
            Zones[selectedZone].unselectBackground();

            selectedZone = newZone;

            Zones[selectedZone].selectBackground();
        }

        private Point getRoundPoint (Point p)
        {
            return new Point(((int)p.X / blockSize) * blockSize, ((int)p.Y / blockSize) * blockSize);
        }

        private void removeBitmapRegion (Point pt, Zone curZone)
        {
            for (int i = 0; i < blockSize; i ++)
                for (int j = 0; j < blockSize; j ++)
                    (curZone.Image as Bitmap).SetPixel(pt.X + i, pt.Y + j, Color.Empty);
        }

        public void showTooltip (Point pt)
        {
            Point roundPt = getRoundPoint(pt);
            String text = "X: " + roundPt.X/16 + ", Y: " + roundPt.Y/16;

            inspectTt.SetToolTip(PictureBox, text);
        }

        #region mapRender

        /** Private helper function to render a newly drawn point onto the screen.
         * Handles async refresh event of the PictureBox in the form */
        private void renderPoint(Point pt)
        {
            /* Determine the size of the update rectangle */
            Rectangle rect = new Rectangle(pt, new Size(blockSize, blockSize));
            using (var screen = Graphics.FromImage(mapScreen))
            {
                /* Render the map with the given update area */
                renderPointHelper(screen, map, rect);

                /* Render every zone with the given update area */
                foreach (Zone zone in Zones)
                    renderPointHelper(screen, zone.Image, rect);
            }

            RpgEE.refreshMap();
        }

        private void renderPointHelper(Graphics screen, Image img, Rectangle rect)
        {
            screen.DrawImage(img, rect, rect, GraphicsUnit.Pixel);
        }

        /** Private helper function to re-render the whole map onto the screen.
         * Handles async refresh event of the PictureBox in the form */
        public void renderMap()
        {
            lock (BackgroundThread._activityLock)
            {
                BackgroundThread.activityQueue.Enqueue(new BackgroundThread.ActionEvent(BackgroundThread.Actions.RenderMap, null));
            }
        }

        public void renderMapBackground()
        {
            using (var screen = Graphics.FromImage(mapScreen))
            {
                /* Render the whole map's image */
                renderMapHelper(screen, map);

                /* Render every zone's entire overlay */
                foreach (Zone zone in Zones)
                    if (zone.Visible)
                        renderMapHelper(screen, zone.Image);
            }
            Console.WriteLine("Background Done!");

            RpgEE.refreshMap();
        }

        public void resetSelectedZone(int x)
        {
            if (selectedZone == x || selectedZone == RpgEE.sideNavListView.Items.Count - 1)
                changeSelectedZone(0);
        }

        private void renderMapHelper(Graphics screen, Image img)
        {
            screen.DrawImage(img, new Point(0, 0));
        }
#endregion
    }
}
