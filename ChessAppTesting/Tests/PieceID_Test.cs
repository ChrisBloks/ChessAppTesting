using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ChessAppTesting.Tests
{
    internal class PieceID_Test
    {
        public static void Run()
        {
            test_1();
        }

        // == Tests ==============================

        private static void test_1()
        {
            // Test parameters
            (TeamColor input1, PieceType input2, int input3) input;
            string expected;

            // Test case 1
            input = (TeamColor.Black, PieceType.Pawn, 1);
            expected = "BP1";
            Debug.Assert(Pieces.PieceID.PreviewID(input.Item1, input.Item2, input.Item3) == expected, "PieceID preview test 1 failed");

            // Test case 2
            input = (TeamColor.White, PieceType.Knight, 5);
            expected = "WN5";
            Debug.Assert(Pieces.PieceID.PreviewID(input.Item1, input.Item2, input.Item3) == expected, "PieceID preview test 2 failed");

            Console.WriteLine("All PieceID preview tests passed.");
        }

        private static void DEP_test_1()
        {
            // Test parameters
            (TeamColor input1, PieceType input2, int input3) input;
            string expected;

            // Test case 1
            input = (TeamColor.Black, PieceType.Pawn, 1);
            expected = "1" + "010" + "100001";
            Debug.Assert(Pieces.PieceID.PreviewID(input.Item1, input.Item2, input.Item3) == expected, "PieceID preview test 1 failed");

            // Test case 2
            input = (TeamColor.White, PieceType.Knight, 5);
            expected = "0" + "011" + "000101";
            Debug.Assert(Pieces.PieceID.PreviewID(input.Item1, input.Item2, input.Item3) == expected, "PieceID preview test 2 failed");

            Console.WriteLine("All PieceID preview tests passed.");
        }
    }
}
