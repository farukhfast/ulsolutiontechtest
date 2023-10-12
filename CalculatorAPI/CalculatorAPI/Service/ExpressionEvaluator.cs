namespace CalculatorAPI.Model
{
    public static class ExpressionEvaluator
    {

        //declaring operators with precedence
        static char[] operatos = { '/', '*', '+', '-' };

        //divistion , mul , add , sub
        public static void EvaluateExpression(ref string expression)
        {
            //get highest precedence operator in input
            char op = GetHighPrecedenceOpInString(expression);

            //get last index of high precedence op occurence
            short indexofHighPrOp = lastIndexOfOp(expression, op);


            //get left right values of that operator
            Dictionary<string, string> pair = GetLeftRightPair(op, indexofHighPrOp, expression);

            string left = "", right = "";

            pair.TryGetValue("left", out left);
            pair.TryGetValue("right", out right);
            string result = "";

            //with respect to operator evaluate it
            switch (op)
            {
                case '/':
                    result = (Parse(left) / Parse(right)).ToString();
                    break;
                case '*':
                    result = (Parse(left) * Parse(right)).ToString();
                    break;
                case '+':
                    result = (Parse(left) + Parse(right)).ToString();
                    break;
                case '-':
                    result = (Parse(left) - Parse(right)).ToString();
                    break;
                default:
                    break;
            }

            //replace user input with result of each like if 4+2*5 then first 2*5 = 10 so 4+10 would be current string
            ReplaceStringWithResult(ref expression, result, indexofHighPrOp);

            // if still operator then recall , recall current function with remaining values to be executed
            if (CheckIfStringContainsOperator(expression))
            {
                //recall in recursive
                EvaluateExpression(ref expression);
            }
        }

        static void ReplaceStringWithResult(ref string expression, string result, short indexofHighPrOp)
        {
            try
            {
                ////since string mutable
                char[] charArray = expression.ToCharArray();

                string evaluatevalue = $"{readLeftTillOperator(expression, indexofHighPrOp)}{charArray[indexofHighPrOp]}{readRightTillOperator(expression, indexofHighPrOp)}";   //$"{charArray[indexofHighPrOp - 1]}{charArray[indexofHighPrOp]}{charArray[indexofHighPrOp + 1]}";

                int index = expression.LastIndexOf(evaluatevalue);

                if (index != -1)
                {
                    string modifiedString = expression.Substring(0, index) + result +
                                            expression.Substring(index + evaluatevalue.Length);
                    expression = modifiedString;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static double Parse(string no)
        {
            try
            {
                double result = double.Parse(no);
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Number not parsable");
            }
        }

        public static string Reverse(string input)
        {
            char[] charArray = input.ToCharArray();
            int length = charArray.Length;

            for (int i = 0; i < length / 2; i++)
            {
                char temp = charArray[i];
                charArray[i] = charArray[length - 1 - i];
                charArray[length - 1 - i] = temp;
            }

            return new string(charArray);
        }
        static string readLeftTillOperator(string expression, int index)
        {
            string result = "";

            char[] array = expression.ToCharArray();
            //since we have to move to left we run loop in inverse
            for (int i = index - 1; i >= 0; i--)
            {
                char element = array[i];

                // Check if the current character is one of the stop characters
                if (operatos.Contains(element))
                {
                    break; // Stop reading when one of the stop characters is encountered
                }

                result += element;

            }

            //taking reverse since if left is 743 in 743+232 then above gives 347 so to correctify it we reverse
            return Reverse(result);

        }

        static string readRightTillOperator(string expression, int index)
        {

            string result = "";

            char[] array = expression.ToCharArray();

            for (int i = index + 1; i < array.Length; i++)
            {
                char element = array[i];
                // Check if the current character is one of the stop characters
                if (operatos.Contains(element))
                {
                    break; // Stop reading when one of the stop characters is encountered
                }

                result += element;
            }


            return result;

        }

        static Dictionary<string, string> GetLeftRightPair(char op, short indexofop, string expression)
        {
            try
            {
                char splitChar = op; // The character at which you want to split the string
                int splitIndex = indexofop;  // The index where you want to split the string

                //failing in case left right char has length >1 
                string left = ""; //expression[indexofop - 1].ToString();
                string right = "";// expression[indexofop + 1].ToString();

                if (splitIndex >= 0 && splitIndex < expression.Length)
                {
                    //lets suppose 234+44 so this will return 234
                    left = readLeftTillOperator(expression, indexofop);
                    //lets suppose 234+44 so this will return 44

                    right = readRightTillOperator(expression, indexofop);
                }

                Dictionary<string, string> pair = new Dictionary<string, string>();

                pair.Add("right", right);
                pair.Add("left", left);
                return pair;
            }
            catch (Exception ex)
            {

                throw new Exception("There is no left right at current operator  , check expression");
            }
        }

        static char GetHighPrecedenceOpInString(string expression)
        {
            foreach (char op in operatos)
            {
                if (expression.Contains(op))
                {
                    return op;
                }
            }
            throw new Exception("No operator match in string");
        }

        static bool CheckIfStringContainsOperator(string expression)
        {

            foreach (char op in operatos)
            {
                if (expression.Contains(op))
                {
                    return true;
                }
            }
            return false;
        }

        static short lastIndexOfOp(string expression, char op)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                return (short)expression.LastIndexOf(op);
            }
            throw new Exception("No operator match at any index");
        }
    }
}
