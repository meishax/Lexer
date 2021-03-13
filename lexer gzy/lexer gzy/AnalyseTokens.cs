using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace lexer_gzy
{
    class AnalyseTokens
    {
        protected string SourceFileName;//源文件
        protected string ResultFileName;//输出文件

        Stack BacketSt = new Stack();//括号栈
        char[] prog = new char[5000];//输入字符暂存数组
        char[] token = new char[100];//标记数组

        char ch;//输入单词
        int p = 0;//单词指针
        int sym = 0;
        int n = 0;//标记指针
        int ErrorLine;//错误行号
        string[] keyword = {
            "if", "else", "while", "signed","throw","union","this",
            "int","char","double","unsigned","const","goto","virtual",
            "for","float","break","auto","class","operator","case",
            "do","long","typedef","static","friend","template","default",
            "new","void","register","extern","return","enum","inline",
            "try","short", "continue","sizeof","switch","private","protected",
            "asm","while","catch","delete","public","volatile","struct",
            "wchar_t","using","typename","typeid","true","static_cast","reubterpret_cast",
            "public","operator","new","namespace","mutable","inline","false",
            "explicit","dynamic_cast","main"};//保留字数组66个
        string[] delimiter = { "//", "/*", "(", ")", "{", "}", "[", "]", "\"", "\'", ";", "," };//界符12个
        string[] operatorstr = { "+", "-", "*", "/", "<", ">", "<=", ">=", "=", "==", "!", "!=", "^", "#", "&", "&&", "|", "||", "%",
            "~", "<<", ">>", "?:", "\\", ".", ":","++","--"};//运算符28个

        public AnalyseTokens()
        {
            Array.Clear(prog, (char)0, 5000);//初始化
        }
        public void setSourceFileName(string file)
        {
            SourceFileName = file;
        }
        public void setResultFileName(string file)
        {
            ResultFileName = file;
        }
        public void space(char ch)
        {
            ch = prog[p++];
            if (ch == ' ' || ch == '\n' || ch == '\t' ||(int)ch == 13||(int)ch==9)
            {
                
                space(ch);
            }
        }
        protected void GetToken()
        {
            ch = prog[p++];
            
            while(ch == ' ' || ch == '\n' || ch == '\t' || (int)ch == 13 || (int)ch == 9)//空格，换行，制表，回车
            {
                ch = prog[p++];
            }
            for (n = 0; n < 100; n++) 
            {
                token[n] = '\0';//标记数组空字符初始化
            }
            n = 0;
            if ((int)ch == 0) 
            {
                return;
            }


            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z' || ch == '_'))//标识符首符号为字母或下划线
            {
                sym = 0;
                do
                {
                    token[n++] = ch;
                    ch = prog[p++];
                }
                while ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z' || ch == '_') || (ch >= '0' && ch <= '9'));//标识符其他符号
                sym = 1;

                string str1 = null;//str1暂存输入标识符与保留字匹配
                for (int i = 0; token[i] != '\0'; i++)
                {
                    str1 += token[i];

                }
                for (int j = 0; j < 66; j++)
                {
                    if (String.Compare(str1, keyword[j]) == 0)
                    {
                        sym = 2 + j;
                    }
                }
                p--;//p为空格后的下一个，pro[p-1]为空格，pro[p]为空格下一个，将p减1
                return;

            }
            else if (ch == '{' || ch == '}' || ch == '(' || ch == ')' || ch == '[' || ch == ']')//如果是界符中的括号
            {
                if (ch == '{')
                {
                    sym = 1 + 66 + 5;
                    BacketSt.Push(ch);
                    token[0] = ch;
                }
                else if (ch == '}')
                {
                    if ((char)BacketSt.Peek() == '{')
                    {
                        sym = 1 + 66 + 6;
                        BacketSt.Pop();
                        token[0] = ch;
                    }
                    else
                    {
                        sym = -1;//括号不匹配
                    }
                }
                else if (ch == '(')
                {
                    sym = 1 + 66 + 3;
                    BacketSt.Push(ch);
                    token[0] = ch;
                }
                else if (ch == ')')
                {
                    if ((char)BacketSt.Peek() == '(')
                    {
                        sym = 1 + 66 + 4;
                        BacketSt.Pop();
                        token[0] = ch;
                    }
                    else
                    {
                        sym = -1;//括号不匹配
                    }
                }
                else if (ch == '[')
                {
                    sym = 1 + 66 + 7;
                    BacketSt.Push(ch);
                    token[0] = ch;

                }
                else if (ch == ']')
                {
                    if ((char)BacketSt.Peek() == '[')
                    {
                        sym = 1 + 66 + 8;
                        BacketSt.Pop();
                        token[0] = ch;
                    }
                    else
                    {
                        sym = -1;//括号不匹配
                    }
                }
                return;
              }








            else if (ch == '+')
            {
                token[n++] = ch;

                //space(ch);
                ch = prog[p++];

                if (ch == '+')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 27;
                }
                else
                {
                    sym = 1 + 66 + 12 + 1;
                    p--;
                }
                return;
            }
            else if (ch == '-')
            {
                token[n++] = ch;

                //space(ch);
                ch = prog[p++];

                if (ch == '-')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 28;
                }
                else
                {
                    sym = 1 + 66 + 12 + 2;
                    p--;
                }
                return;
            }
            else if (ch == '>')
            {
                token[n++] = ch;

               // space(ch);
                ch = prog[p++];

                if (ch == '>')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 22;
                }
                else
                {
                    sym = 1 + 66 + 12 + 6;
                    p--;
                }
                return;
            }
            else if (ch == '<')
            {
                token[n++] = ch;

                //space(ch);
                ch = prog[p++];

                if (ch == '<')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 21;
                }
                else
                {
                    sym = 1 + 66 + 12 + 5;
                    p--;
                }
                return;
            }
            else if (ch == '=')
            {
                token[n++] = ch;

               // space(ch);
                ch = prog[p++];

                if (ch == '=')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 10;
                }
                else
                {
                    sym = 1 + 66 + 12 + 9;
                    p--;
                }
                return;
            }
            else if (ch == '|')
            {
                token[n++] = ch;

               // space(ch);
                ch = prog[p++];

                if (ch == '|')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 18;
                }
                else
                {
                    sym = 1 + 66 + 12 + 17;
                    p--;
                }
                return;
            }
            else if (ch == '=')
            {
                token[n++] = ch;

                //space(ch);
                ch = prog[p++];

                if (ch == '=')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 10;
                }
                else
                {
                    sym = 1 + 66 + 12 + 9;
                    p--;
                }
                return;
            }
            else if (ch == '&')
            {
                token[n++] = ch;

                //space(ch);
                ch = prog[p++];

                if (ch == '&')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 16;
                }
                else
                {
                    sym = 1 + 66 + 12 + 15;
                    p--;
                }
                return;
            }
            else if (ch == '!')
            {
                token[n++] = ch;

                //space(ch);
                ch = prog[p++];

                if (ch == '=')
                {
                    token[n++] = ch;
                    sym = 1 + 66 + 12 + 12;
                }
                else if (ch == ',' || ch == ';')
                {
                    sym = -1;
                }
                else
                {
                    sym = 1 + 66 + 12 + 11;
                    p--;
                }
                return;
            }
            else if (ch == '>' || ch == '<')
            {
                token[n++] = ch;
                char c = ch;
                ch = prog[p++];
                //space(ch);
                if (ch == '=')
                {
                    if (c == '<')
                    {
                        sym = 1 + 66 + 12 + 7;
                    }
                    if (c == '>')
                    {
                        sym = 1 + 66 + 12 + 8;
                    }
                    token[n++] = ch;
                }
                else
                {
                    if (c == '<')
                    {
                        sym = 1 + 66 + 12 + 5;
                    }
                    if (c == '>')
                    {
                        sym = 1 + 66 + 12 + 6;
                    }
                    p--;
                }
                return;

            }
            else if (ch >= '0' && ch <= '9')
            {
                sym = 1 + 66 + 12 + 28 + 1;
                do
                {
                    token[n++] = ch;
                    ch = prog[p++];
                    //space(ch);
                } while (ch >= '0' && ch <= '9');
                sym = 1 + 66 + 12 + 28 + 1;
                if (ch != ','&& ch != ' ' && ch != '+' && ch != '-' && ch != '*' && ch != '/' && ch != ';' && ch != '\n' && ch != '\t' && (int)ch != 13 && (int)ch != 9 && ch != '{' && ch != '(' && ch != '[' && ch != '<' && ch != '>' && ch != '=' && ch != '}' && ch != ')' && ch != ']')
                {
                    do
                    {
                        token[n++] = ch;
                        ch = prog[p++];
                    } while (!(ch == ' ' || ch == '\n' || ch == '\t' || (int)ch == 13 || (int)ch == 9));
                    sym = -1;
                    LocateError();
                }
                p--;
                return;
            }
            else
            {

                token[0] = ch;
                string str2 = null;
                str2 += token[0];
                for (n = 8; n < 12; n++)
                {
                    if (String.Compare(str2, delimiter[n]) == 0)
                    {
                        sym = 1 + 66 + n + 1;
                    }

                }
                for (n = 0; n < 28; n++)
                {
                    if (String.Compare(str2, operatorstr[n]) == 0)
                    {
                        sym = 1 + 66 + 12 + n + 1;
                    }
                }

                if (sym <= 66)
                {
                    sym = -2;
                    MessageBox.Show("The input contains illegal characters. 输入内容包含不合法字符");
                }
            }
           


        }
        public void readSorceFile()
        {
            FileStream aFile = new FileStream(SourceFileName, FileMode.Open);
            StreamReader sr = new StreamReader(aFile);
            FileStream fs = new FileStream(ResultFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            do
            {
                ch = (char)sr.Read();
                if (ch == '/' && sr.Peek() == '/')
                {
                    string c = sr.ReadLine();
                    ch = '\n';
                }
                if (ch == '/' && sr.Peek() == '*')
                {
                    do
                    {
                        ch = (char)sr.Read();

                    } while (!(ch == '*' && sr.Peek() == '/'));
                    ch = (char)sr.Read();
                    ch = (char)sr.Read();
                }
                prog[p++] = ch;
            } while (sr.Peek() >= 0);
      
            p = 0;
            do
            {
                GetToken();
                if ((int)ch == 0)
                {
                    break;
                }
                string str1 = null;
                for (int i = 0; token[i] != '\0'; i++)
                {
                    str1 += token[i];
                }
                if (str1 != null)
                {
                    switch (sym)
                    {
                        case -1:
                            string str2 = "Line" + ErrorLine.ToString() + " **Error occurs in " + str1;
                            sw.WriteLine(str2);
                            Console.WriteLine(str2);
                            break;
                        case -2:
                            string str3 = '(' + sym.ToString() + ',' + "Error" + ')';
                            sw.WriteLine(str3);
                            Console.WriteLine(str3);
                            break;
                        default:
                            string str = '(' + sym.ToString() + ',' + str1 + ')';
                            sw.WriteLine(str);
                            Console.WriteLine(str);
                            break;

                    }
                }
            } while (ch > 0);
            sr.Close();
            sw.Close();
            Console.WriteLine("File closed.");


        }
        void LocateError()
        {
            string str1 = null;
            for (int i = 0; token[i] != '\0'; i++)
            {
                str1 += token[i];
            }
            int j = 0;
            while(!string.IsNullOrWhiteSpace(词法分析器.temp[j]))
            {
                int i = 词法分析器.temp[j++].IndexOf(str1);
                if (i > 0)
                {
                    ErrorLine = j;
                    break;
                }
            }
        }
        

 

    }
}
