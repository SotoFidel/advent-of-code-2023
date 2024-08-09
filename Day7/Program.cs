using System.Collections.Frozen;
using System.Collections.Specialized;
using System.ComponentModel;

class Program
{

    class Hand(string cards, int bid)
    {
        public string Cards { get; set; } = cards;
        public int Bid { get; set; } = bid;
        public int Rank { get; set; } = 0;
        public int HandType { get; set; } = -1;

    }
    static void Main(string[] args)
    {

        if (args.Length != 2)
        {
            Console.WriteLine("Usage: [Part1|Part2] [FileName]");
            return;
        }

        switch (args[0])
        {
            case "Part1":
                Part1(args[1]);
                break;
            case "Part2":
                Part2(args[1]);
                break;
            default:
                Console.WriteLine("Usage: [Part1|Part2] [FileName]");
                return;
        }

    }

    static void Part1(string fileName)
    {
        (StreamReader? streamReader, Exception? exception) = OpenFile(fileName);
        if (exception != null)
        {
            Console.WriteLine(GetFullError(exception));
            return;
        }

        Dictionary<char,int> cards = new()
        {
            {'2',2}, {'3',3}, {'4',4}, {'5',5},
            {'6',6}, {'7',7}, {'8',8}, {'9',9},
            {'T',10}, {'J',11}, {'Q',12}, {'K',13},
            {'A',14}
        };

        List<List<Hand>> handsGrouped =
            streamReader!.ReadToEnd().Split('\n')
            .Select(line => {
                string[] elements = line.Split(' ');
                if(elements[0].Length < 5)
                {
                    throw new Exception("ahhh");
                }
                return new Hand(elements[0],int.Parse(elements[1]));
            })
            .ToList()
            .GroupBy(hand => {
                int matchNum;
                int numPairs = 0;
                string originalHand = hand.Cards;

                /*
                    0: High Cards | 1: One Pair
                    2: Two Pair   | 3; Three of a kind
                    4: Full House | 5: Four of a kind
                    6: Five of a kind
                */
                foreach(char c in originalHand)
                {
                    matchNum = originalHand.Where(handChar => c == handChar).Count();
                    switch(matchNum)
                    {
                        case 5:
                        case 4:
                            hand.HandType = matchNum + 1;
                            return hand.HandType;
                        case 3:
                            if(originalHand.Where(dc => dc != c).Distinct().Count() == 2)
                            {
                                hand.HandType = 3;
                            }
                            else
                            {
                                hand.HandType = 4;
                            }
                            return hand.HandType;
                        case 2:
                            numPairs++;
                            originalHand = originalHand.Replace(c.ToString(),"");
                            break;
                    }
                }

                hand.HandType = numPairs;
                return hand.HandType;

            })
            .OrderBy(group => group.Key)
            .Select(group => group.ToList()).ToList();

        int rank = 1;
        long totalWinnings = 0;
        Hand temp;
        foreach(List<Hand> handGroup in handsGrouped)
        {
            for(int pass = 0; pass < handGroup.Count - 1; pass++)
            {
                for(int hand = 0; hand < handGroup.Count - pass - 1; hand++)
                {
                    for(int character = 0; character < handGroup[hand].Cards.Length; character++)
                    {
                        if(cards[handGroup[hand].Cards[character]] > cards[handGroup[hand + 1].Cards[character]])
                        {
                            temp = handGroup[hand];
                            handGroup[hand] = handGroup[hand + 1];
                            handGroup[hand + 1] = temp;
                            break;
                        }
                        else if(cards[handGroup[hand].Cards[character]] < cards[handGroup[hand + 1].Cards[character]])
                        {
                            break;
                        }
                    }
                }           
            }

            for(int hand = 0; hand < handGroup.Count; hand++)
            {
                handGroup[hand].Rank = rank++;
                totalWinnings += handGroup[hand].Rank * handGroup[hand].Bid;
            }
        }

        Console.WriteLine("Total Winnings: " + totalWinnings);

        return;

    }

    static void Part2(string fileName)
    {
        (StreamReader? streamReader, Exception? exception) = OpenFile(fileName);
        if (exception != null)
        {
            Console.WriteLine(GetFullError(exception));
            return;
        }

        Dictionary<char,int> cards = new()
        {
            {'J',1},{'2',2}, {'3',3}, {'4',4}, {'5',5},
            {'6',6}, {'7',7}, {'8',8}, {'9',9},
            {'T',10},{'Q',11}, {'K',12},{'A',13}
        };

        List<List<Hand>> handsGrouped =
            streamReader!.ReadToEnd().Split('\n')
            .Select(line => {
                string[] elements = line.Split(' ');
                if(elements[0].Length < 5)
                {
                    throw new Exception("ahhh");
                }
                return new Hand(elements[0],int.Parse(elements[1]));
            })
            .ToList()
            .GroupBy(hand => {
                int matchNum;
                int numPairs = 0;
                string originalHand = hand.Cards;

                /*
                    | 0: High Cards | 1: One Pair
                    | 2: Two Pair   | 3; Three of a kind 
                    | 4: Full House | 5: Four of a kind
                    | 6: Five of a kind
                */
                foreach(char c in originalHand)
                {
                    matchNum = originalHand.Where(handChar => c == handChar).Count();
                    switch(matchNum)
                    {
                        case 5:
                            hand.HandType = 6;
                            return hand.HandType;
                        case 4:
                            hand.HandType = 5;
                            if (c == 'J' || hand.Cards.Contains('J')) { hand.HandType = 6; } // Promote to 5 of a kind
                            return hand.HandType;
                        case 3:
                            if (c == 'J')
                            {
                                hand.HandType = 5; // Promote to 4 of a kind
                                if (hand.Cards.Where(dc => dc != 'J').Distinct().Count() == 1)
                                {
                                    hand.HandType = 6;
                                }
                            }
                            else if(hand.Cards.Where(dc => dc != c).Distinct().Count() == 2)
                            {
                                hand.HandType = 3;
                                if(hand.Cards.Where(dc => dc == 'J').Count() == 1)
                                {
                                    hand.HandType = 5;
                                }
                            }
                            else
                            {
                                hand.HandType = 4;
                                if(hand.Cards.Where(dc => dc == 'J').Count() == 2)
                                {
                                    hand.HandType = 6;
                                }
                            }
                            return hand.HandType;
                        case 2:
                            numPairs++;
                            originalHand = originalHand.Replace(c.ToString(),"");
                            break;
                    }
                }

                switch(numPairs)
                {
                    case 2:
                        if (hand.Cards.Where(c => c == 'J').Count() == 2)
                        {
                            // One of the Pairs was JJ. We can replace those
                            // and promote to 4 of a kind, which will combine with
                            // the other pair of characters
                            hand.HandType = 5;
                        }
                        else if (hand.Cards.Where(c => c == 'J').Count() == 1)
                        {
                            // TTAAJ => TTAAA or TTAAT. Full house in any case
                            hand.HandType = 4;
                        }
                        else
                        {
                            hand.HandType = numPairs;
                        }
                        break;
                    case 1:
                        if(hand.Cards.Where(c => c == 'J').Count() == 2 || hand.Cards.Where(c => c == 'J').Count() == 1)
                        {
                            hand.HandType = 3;
                        }
                        else
                        {
                            hand.HandType = numPairs;
                        }
                        break;
                    case 0:
                        if(hand.Cards.Contains('J'))
                        {
                            numPairs++;
                        }
                        hand.HandType = numPairs;
                        break;
                    default:
                        throw new Exception("aaaaahhh");
                }

                return hand.HandType;

            })
            .OrderBy(group => group.Key)
            .Select(group => group.ToList()).ToList();

        int rank = 1;
        long totalWinnings = 0;
        Hand temp;
        foreach(List<Hand> handGroup in handsGrouped)
        {
            for(int pass = 0; pass < handGroup.Count - 1; pass++)
            {
                for(int hand = 0; hand < handGroup.Count - pass - 1; hand++)
                {
                    for(int character = 0; character < handGroup[hand].Cards.Length; character++)
                    {
                        if(cards[handGroup[hand].Cards[character]] > cards[handGroup[hand + 1].Cards[character]])
                        {
                            temp = handGroup[hand];
                            handGroup[hand] = handGroup[hand + 1];
                            handGroup[hand + 1] = temp;
                            break;
                        }
                        else if(cards[handGroup[hand].Cards[character]] < cards[handGroup[hand + 1].Cards[character]])
                        {
                            break;
                        }
                    }
                }           
            }

            for(int hand = 0; hand < handGroup.Count; hand++)
            {
                handGroup[hand].Rank = rank++;
                totalWinnings += handGroup[hand].Rank * handGroup[hand].Bid;
            }
        }

        Console.WriteLine("Total Winnings: " + totalWinnings);

        return;

    }

    static Tuple<StreamReader?,Exception?> OpenFile(string fileName)
    {
        try
        {
            StreamReader streamReader = new StreamReader(fileName);
            return new (streamReader, null);
        }
        catch (Exception e)
        {
            return new (null, e);
        }
    }

    static string GetFullError(Exception? ex)
    {
        if (ex == null) { return ""; }
        return ex.InnerException == null
                ? ex.Message
                : ex.Message + " --> " + GetFullError(ex.InnerException);
    }
}