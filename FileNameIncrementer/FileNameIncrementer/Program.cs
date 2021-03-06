﻿using System;
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
            Console.WriteLine("files must be in the format \"filenameXX\"");
            bool keepGoing = true;
            while (keepGoing)
            {
                Console.WriteLine();
                switch (CIO.PromptForMenuSelection(new string[] { "Increment files with 2 trailing numbers", "Increment folders with 2 trailing numbers", "Convert trailing XX into numbers" }, true))
                {
                    case 1:
                        Increment();
                        break;
                    case 2:
                        IncrementFolders();
                        break;
                    case 3:
                        Change();
                        break;
                    case 0:
                        keepGoing = false;
                        break;
                }
            }
        }

        private static void IncrementFolders()
        {
            string folderPath = "";
            FolderBrowserDialog f = new FolderBrowserDialog();
            DialogResult d = f.ShowDialog();

            if (d == DialogResult.OK)
            {
                folderPath = f.SelectedPath;
                string[] folders = Directory.GetDirectories(folderPath);
                if (folders.Count() != 0)
                {

                    if (CIO.PromptForBool($"{folders.Count()} folders found. Continue? (y/n)", "y", "n"))
                    {
                        int inc = CIO.PromptForInt("How much do you want to increment each folder by?", 0, int.MaxValue);
                        List<string> names = new List<string>();
                        foreach (string folder in folders)
                        {
                            try
                            {
                                int num = int.Parse(folder.Substring(folder.Length - 2, 2));
                                num += inc;
                                string folderBeginning = folder.Substring(0,folder.Length-2);
                                string[] path = folder.Split('\\');
                                string name = path[path.Count() - 1];
                                name = name.Substring(0,name.Length-2);
                                Directory.CreateDirectory(folderPath + "\\TEMPFOLDERDONOTTOUCHWHILEPROGRAMISRUNNINGYEEEEEEET");
                                Directory.Move(folder, folderPath + $"\\TEMPFOLDERDONOTTOUCHWHILEPROGRAMISRUNNINGYEEEEEEET\\{name}{(num <= 9 ? "0" + num : num.ToString())}");
                                names.Add(folderPath + $"\\TEMPFOLDERDONOTTOUCHWHILEPROGRAMISRUNNINGYEEEEEEET\\{name}{(num <= 9 ? "0" + num : num.ToString())}");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{folder} will be skipped: " + e.Message);
                            }
                        }
                        foreach (string path in names)
                        {
                            //no catches
                            Directory.Move(path, folderPath + $"\\{(path.Split('\\')[path.Split('\\').Count() - 1])}");


                        }
                        if (names.Count > 0)
                        {
                            Directory.Delete(Directory.GetParent(names[0].Split('.')[0]).FullName);
                        }
                        //
                    }
                }
                else
                {
                    Console.WriteLine("No Folders found");
                }
            }

        }

        public static void Increment()
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
                                string prefix = file.Split('.')[0].Substring(0, file.Split('.')[0].Length - 2);
                                string suffix = "." + file.Split('.')[1];
                                int newNum = int.Parse(file.Substring(prefix.Length, 2));
                                newNum += inc;
                                string t = prefix + (newNum <= 9 ? $"0{newNum}" : newNum.ToString()) + suffix;
                                string[] tempfilepath = t.Split(new string[] { "\\" }, StringSplitOptions.None);
                                string tempLocation = "";
                                for (int i = 0; i < tempfilepath.Count() - 1; i++)
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
                                Console.WriteLine($"{file} will be skipped: " + e.Message);
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
                                Console.WriteLine(filepath + "\\" + name + " already exists, skipping, check folder for manual movement");
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

        }
        public static void Change()
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
                        int inc = CIO.PromptForInt("What number do you want to start at?", 0, int.MaxValue);
                        List<string> names = new List<string>();
                        for (int val = 0; val < files.Count(); val++)
                        {
                            try
                            {
                                if (files[val].Split('.')[0].Substring(files[val].Split('.')[0].Length - 2) != "XX")
                                {
                                    throw new Exception($"does not end in XX");
                                }
                                string prefix = files[val].Split('.')[0].Substring(0, files[val].Split('.')[0].Length - 2);
                                string suffix = "." + files[val].Split('.')[1];
                                string t = prefix + (val + inc <= 9 ? $"0{val + inc}" : (val + inc).ToString()) + suffix;
                                string[] tempfilepath = t.Split(new string[] { "\\" }, StringSplitOptions.None);
                                string tempLocation = "";
                                for (int i = 0; i < tempfilepath.Count() - 1; i++)
                                {
                                    tempLocation += tempfilepath[i] + "\\";
                                }
                                tempLocation += "TEMPFOLDERFORPROGRAMDONOTCHANGEYEEEEEEEEEET\\";
                                Directory.CreateDirectory(tempLocation);
                                tempLocation += tempfilepath[tempfilepath.Count() - 1];
                                names.Add(tempLocation);
                                File.Move(files[val], tempLocation);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{files[val]} will be skipped: " + e.Message);
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
                                Console.WriteLine(filepath + "\\" + name + " already exists, skipping, check folder for manual movement");
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
        }
    }
}
