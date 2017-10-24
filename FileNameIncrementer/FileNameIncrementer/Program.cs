using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using CSC160_ConsoleMenu;
using System.IO;

namespace FileNameIncrementer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Increments filenames with 2 trailing numbers by a specified amount");
            bool keepGoing = true;
            while (keepGoing)
            {
                string folderPath = "";
                FolderBrowserDialog f = new FolderBrowserDialog();
                DialogResult d = f.ShowDialog();
                if (d == DialogResult.OK)
                {
                    folderPath = f.SelectedPath;
                    string[] files = Directory.GetFiles(folderPath);
                    if (files.Count() != 0)
                    {

                        if (CIO.PromptForBool($"{files.Count()} files found. Continue? (y/n)", "y", "n"))
                        {
                            int inc = CIO.PromptForInt("How much do you want to increment each file by?", 0, int.MaxValue);
                            List<string> names = new List<string>();
                            foreach (string file in files)
                            {
                                try
                                {
                                    string prefix = file.Split('.')[0].Substring(0,file.Split('.')[0].Length-2);
                                    string suffix = "."+file.Split('.')[1];
                                    int newNum = int.Parse(file.Substring(prefix.Length, 2));
                                    newNum += inc;
                                    string t=prefix+(newNum<=9?$"0{newNum}":newNum.ToString())+suffix;
                                    string[] tempfilepath = t.Split(new string[] {"\\"},StringSplitOptions.None);
                                    string tempLocation ="";
                                    for(int i = 0; i < tempfilepath.Count() - 1; i++)
                                    {
                                        tempLocation += tempfilepath[i] + "\\";
                                    }
                                    tempLocation += "TEMPFOLDERFORPROGRAMDONOTCHANGEYEEEEEEEEEET\\";
                                    Directory.CreateDirectory(tempLocation);
                                    tempLocation += tempfilepath[tempfilepath.Count() - 1];
                                    names.Add(tempLocation);
                                    File.Move(file, tempLocation);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"{file} will be skipped: "+e.Message);
                                }
                            }
                            foreach (string path in names)
                            {
                                string filepath = path.Split('.')[0];
                                string name = path.Split(new string[] { "\\" }, StringSplitOptions.None).Last();
                                filepath = Directory.GetParent(Directory.GetParent(filepath).FullName).FullName;
                                if (!File.Exists(filepath + "\\" + name))
                                {
                                    File.Move(path, filepath + "\\" + name);
                                }
                                else
                                {
                                    Console.WriteLine(filepath + "\\" + name +" already exists, skipping, check folder fo manual movement");
                                }


                            }
                            if (names.Count > 0)
                            {
                                Directory.Delete(Directory.GetParent(names[0].Split('.')[0]).FullName);
                            }
                            
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Files found");
                    }


                }














                keepGoing = CIO.PromptForBool("Increment another folder? (y/n)", "y", "n");
            }
        }
    }
}
