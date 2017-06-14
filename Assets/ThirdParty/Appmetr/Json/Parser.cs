/*
 * Copyright (c) 2013 Calvin Rien
 *
 * Based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * Simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Appmetr.Json
{
    public class Parser : IDisposable
    {
        const string WordBreak = "{}[],:\"";

        public static bool IsWordBreak(char c)
        {
            return Char.IsWhiteSpace(c) || WordBreak.IndexOf(c) != -1;
        }

        enum Token
        {
            None,
            CurlyOpen,
            CurlyClose,
            SquaredOpen,
            SquaredClose,
            Colon,
            Comma,
            String,
            Number,
            True,
            False,
            Null
        };

        StringReader _json;

        Parser(string jsonString)
        {
            _json = new StringReader(jsonString);
        }

        public static object Parse(string jsonString)
        {
            using(var instance = new Parser(jsonString))
            {
                return instance.ParseValue();
            }
        }

        public void Dispose()
        {
            _json.Dispose();
            _json = null;
        }

        Dictionary<string, object> ParseObject()
        {
            Dictionary<string, object> table = new Dictionary<string, object>();

            // ditch opening brace
            _json.Read();

            // {
            while(true)
            {
                switch(NextToken)
                {
                    case Token.None:
                        return null;
                    case Token.Comma:
                        continue;
                    case Token.CurlyClose:
                        return table;
                    default:

                        // name
                        string name = ParseString();
                        if(name == null)
                        {
                            return null;
                        }

                        // :
                        if(NextToken != Token.Colon)
                        {
                            return null;
                        }

                        // ditch the colon
                        _json.Read();

                        // value
                        table[name] = ParseValue();
                        break;
                }
            }
        }

        List<object> ParseArray()
        {
            List<object> array = new List<object>();

            // ditch opening bracket
            _json.Read();

            // [
            var parsing = true;
            while(parsing)
            {
                Token nextToken = NextToken;

                switch(nextToken)
                {
                    case Token.None:
                        return null;
                    case Token.Comma:
                        continue;
                    case Token.SquaredClose:
                        parsing = false;
                        break;
                    default:
                        object value = ParseByToken(nextToken);
                        array.Add(value);
                        break;
                }
            }

            return array;
        }

        object ParseValue()
        {
            Token nextToken = NextToken;
            return ParseByToken(nextToken);
        }

        object ParseByToken(Token token)
        {
            switch(token)
            {
                case Token.String:
                    return ParseString();
                case Token.Number:
                    return ParseNumber();
                case Token.CurlyOpen:
                    return ParseObject();
                case Token.SquaredOpen:
                    return ParseArray();
                case Token.True:
                    return true;
                case Token.False:
                    return false;
                case Token.Null:
                    return null;
                default:
                    return null;
            }
        }

        string ParseString()
        {
            StringBuilder s = new StringBuilder();
            char c;

            // ditch opening quote
            _json.Read();

            while(true)
            {
                if(_json.Peek() == -1)
                {
                    break;
                }

                c = NextChar;
                switch(c)
                {
                    case '"':
                        break;
                    case '\\':
                        if(_json.Peek() == -1)
                        {
                            break;
                        }

                        c = NextChar;
                        switch(c)
                        {
                            case '"':
                            case '\\':
                            case '/':
                                s.Append(c);
                                break;
                            case 'b':
                                s.Append('\b');
                                break;
                            case 'f':
                                s.Append('\f');
                                break;
                            case 'n':
                                s.Append('\n');
                                break;
                            case 'r':
                                s.Append('\r');
                                break;
                            case 't':
                                s.Append('\t');
                                break;
                            case 'u':
                                var hex = new char[4];
                                for(int i = 0; i < 4; i++)
                                {
                                    hex[i] = NextChar;
                                }

                                s.Append((char) Convert.ToInt32(new string(hex), 16));
                                break;
                        }

                        break;
                    default:
                        s.Append(c);
                        break;
                }
            }

            return s.ToString();
        }

        object ParseNumber()
        {
            string number = NextWord;

            if(number.IndexOf('.') == -1)
            {
                long parsedInt;
                Int64.TryParse(number, out parsedInt);
                return parsedInt;
            }

            double parsedDouble;
            Double.TryParse(number, out parsedDouble);
            return parsedDouble;
        }

        void EatWhitespace()
        {
            while(Char.IsWhiteSpace(PeekChar))
            {
                _json.Read();

                if(_json.Peek() == -1)
                {
                    break;
                }
            }
        }

        char PeekChar
        {
            get { return Convert.ToChar(_json.Peek()); }
        }

        char NextChar
        {
            get { return Convert.ToChar(_json.Read()); }
        }

        string NextWord
        {
            get
            {
                StringBuilder word = new StringBuilder();

                while(!IsWordBreak(PeekChar))
                {
                    word.Append(NextChar);

                    if(_json.Peek() == -1)
                    {
                        break;
                    }
                }

                return word.ToString();
            }
        }

        Token NextToken
        {
            get
            {
                EatWhitespace();

                if(_json.Peek() == -1)
                {
                    return Token.None;
                }

                switch(PeekChar)
                {
                    case '{':
                        return Token.CurlyOpen;
                    case '}':
                        _json.Read();
                        return Token.CurlyClose;
                    case '[':
                        return Token.SquaredOpen;
                    case ']':
                        _json.Read();
                        return Token.SquaredClose;
                    case ',':
                        _json.Read();
                        return Token.Comma;
                    case '"':
                        return Token.String;
                    case ':':
                        return Token.Colon;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return Token.Number;
                }

                switch(NextWord)
                {
                    case "false":
                        return Token.False;
                    case "true":
                        return Token.True;
                    case "null":
                        return Token.Null;
                }

                return Token.None;
            }
        }
    }
}