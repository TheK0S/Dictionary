
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;

bool menu = true;
bool switchMenu = true;
int mainMenu;
int dictionaryIndex;
string json;

List<MyDictionary>? Dictionaris = new List<MyDictionary>();

try
{
    string[] dictionaryPathArray = Directory.GetFiles(Directory.GetCurrentDirectory(), "*-*.json");

    for (int i = 0; i < dictionaryPathArray.Length; i++)
    {
        for (int j = dictionaryPathArray[i].Length - 6; j > 0; j--)
        {
            if (dictionaryPathArray[i][j] == '\\')
            {
                string dictionaryName = dictionaryPathArray[i].Remove(0,j+1);
                dictionaryName = dictionaryName.Substring(0, dictionaryName.Length - 5);
                Dictionaris.Add(new MyDictionary(dictionaryName,JsonConvert.DeserializeObject<List<Word>>(File.ReadAllText(dictionaryPathArray[i]))));
                break;
            }
        }
    }
}
catch (Exception)
{
    Console.WriteLine("Ошибка чтения файла данных");
}

string word, translate;

while (menu)
{
    while (true)
    {
        Console.WriteLine("Введите свой выбор");
        Console.WriteLine("1 - Добавить словарь");
        Console.WriteLine("2 - Работать с словарем");
        Console.WriteLine("0 - Выход");

        if (int.TryParse(Console.ReadLine(), out mainMenu) && mainMenu >= 0 && mainMenu <= 2)
            break;
        else
            Console.WriteLine("Ошибка ввода!");
    }

    Console.Clear();
    if (mainMenu == 0) break;

    if (mainMenu == 1)
    {
        Console.WriteLine("Введите назвиние для соваря:\n(Например Англо - Украинский)");
        word = Console.ReadLine() ?? "";
        if (word.Length > 0)
            Dictionaris.Add(new MyDictionary(word));
        else
            Console.WriteLine("Произошла ошибка! Запись не добавлена!");
    }

    else if (mainMenu == 2)
    {
        if (Dictionaris.Count == 0)
        {
            Console.WriteLine("Словарей нет. Попробуйте добавить новый словарь");
            continue;
        }

        while (true)
        {
            Console.WriteLine("Выберите словарь");

            for (int i = 0; i < Dictionaris.Count; i++)
            {
                Console.WriteLine($"{i} - {Dictionaris[i].Name}");
            }

            if (int.TryParse(Console.ReadLine(), out dictionaryIndex) && dictionaryIndex >= 0 && dictionaryIndex < Dictionaris.Count)
                break;
            else
                Console.WriteLine("Ошибка ввода!");
        }
        while (switchMenu)
        {
            while (true)
            {
                Console.WriteLine("\nВведите необходимый пункт");
                Console.WriteLine("1 - Добавить слово в словарь");
                Console.WriteLine("2 - Добавить перевод к слову в словаре");
                Console.WriteLine("3 - Удалить слово из словаря");
                Console.WriteLine("4 - Удалить один из вариантов перевода слова");
                Console.WriteLine("5 - Изменить слово в словаре");
                Console.WriteLine("6 - Искать слово в словаре по одной или несколько буквам");
                Console.WriteLine("7 - Искать слово по переводу");
                Console.WriteLine("8 - Экспортировать слово в файл");
                Console.WriteLine("0 - Выход");

                if (int.TryParse(Console.ReadLine(), out mainMenu) && mainMenu >= 0 && mainMenu <= 8)
                    break;
                else
                    Console.WriteLine("Ошибка ввода!");
            }
            switch ((Menu)mainMenu)
            {
                case Menu.Exit:
                    switchMenu = false;
                    menu = false;
                    break;

                case Menu.AddWord:
                    Console.WriteLine("Введите слово");
                    word = Console.ReadLine() ?? "";
                    Console.WriteLine("Введите перевод слова");
                    translate = Console.ReadLine() ?? "";

                    Dictionaris[dictionaryIndex].AddWord(word, translate);
                    break;

                case Menu.AddTranslate:
                    Console.WriteLine("Введите слово, которому хотите добавить перевод");
                    word = Console.ReadLine() ?? "";
                    Console.WriteLine("Введите перевод слова");
                    translate = Console.ReadLine() ?? "";

                    if (Dictionaris[dictionaryIndex].AddTranslate(word, translate))
                        Console.WriteLine("Запись добавлена");
                    else
                        Console.WriteLine("Произошла ошибка! Запись не добавлена!");

                    break;

                case Menu.RemoveWord:
                    Console.WriteLine("Введите слово, которое хотите удалить из словаря");

                    if (Dictionaris[dictionaryIndex].RemoveWord(Console.ReadLine() ?? ""))
                        Console.WriteLine("Запись удалена");
                    else
                        Console.WriteLine("Произошла ошибка! Запись не удалена!");

                    break;

                case Menu.RemoveTranslate:
                    Console.WriteLine("Введите слово, перевод которого хотите удалить");
                    word = Console.ReadLine() ?? "";
                    Console.WriteLine("Введите перевод, который хотите удалить");
                    translate = Console.ReadLine() ?? "";

                    if (Dictionaris[dictionaryIndex].RemoveTranslate(word, translate))
                        Console.WriteLine("Запись удалена");
                    else
                        Console.WriteLine("Произошла ошибка! Запись не удалена!");

                    break;

                case Menu.ChangeWord:
                    Console.WriteLine("Введите слово, которое хотите изменить");
                    word = Console.ReadLine() ?? "";
                    Console.WriteLine("Введите новое слово");

                    if (Dictionaris[dictionaryIndex].ChangeWord(word, Console.ReadLine() ?? ""))
                        Console.WriteLine("Запись изменена");
                    else
                        Console.WriteLine("Произошла ошибка! Запись не изменена!");

                    break;
                case Menu.SearchWord:
                    Console.WriteLine("Введите первую букву, несколько букв начала или слово целеком, которое хотите найти в словаре");
                    Console.WriteLine("Слово должно начинатся с большой буквы");
                    Dictionaris[dictionaryIndex].PrintWords(Console.ReadLine() ?? "");

                    break;

                case Menu.SearchTranslate:
                    Console.WriteLine("Введите перевод для поиска в словаре");

                    Word result = Dictionaris[dictionaryIndex].GetWord(Dictionaris[dictionaryIndex].SearchTranslate(Console.ReadLine() ?? ""));

                    if (result.OriginalWord != "0")
                        result.ShowWord();
                    else
                        Console.WriteLine("Запись не найдена!");

                    break;

                case Menu.ExportWord:
                    Console.WriteLine("Введите слово, которое хотите записать в файл");
                    int index = Dictionaris[dictionaryIndex].SearchWord(Console.ReadLine() ?? "");

                    if (index == -1)
                    {
                        Console.WriteLine("Слово не найдено");
                        break;
                    }

                    Word wordForJson = Dictionaris[dictionaryIndex].GetWord(index);


                    Console.WriteLine("Введите имя для файла экспорта");
                    string pathWordExport = Console.ReadLine() ?? "export_word";
                    pathWordExport += ".json";

                    json = JsonConvert.SerializeObject(wordForJson);
                    File.WriteAllText(pathWordExport, json);

                    break;

                default:
                    break;
            }
        }
    }

    if (!menu)
    {
        for (int i = 0; i < Dictionaris?.Count; i++)
            Dictionaris[i].Serialize(i);

        Console.WriteLine("Данные сохранены успешно");
    }

}

public enum Menu
{
    Exit, AddWord, AddTranslate, RemoveWord, RemoveTranslate, ChangeWord,SearchWord, SearchTranslate, ExportWord
}

public class Word
{
    public List<string> translations = new List<string>();
    public string OriginalWord { get; set; }

    public Word(string originalWord, string translate)
    {
        OriginalWord = originalWord;
        translations.Add(translate);
    }

    public bool AddTranslate(string translate)
    {
        if (translate.Length == 0)
            return false;
        foreach (var item in translations)
        {
            if (item == translate)
                return false;
        }

        translations.Add(translate);
        return true;
    }

    public void ShowWord()
    {
        Console.WriteLine($"Оригинальное слово: {OriginalWord}");
        Console.WriteLine("Перевод:");

        foreach (var item in translations)
            Console.WriteLine(item);
    }

    public bool ChangeTranslate(string oldTranslate, string newTranslate)
    {
        int index = translations.IndexOf(oldTranslate);
        if (index == -1)
            return false;

        translations[index] = newTranslate;
        return true;
    }

    public bool RemoveTranslate(string translate)
    {
        if (translations.Count < 2)
            return false;

        return translations.Remove(translate);
    }
}

public class MyDictionary
{
    List<Word> words;
    public string Name { get; set; }

    public MyDictionary(string name)
    {
        words = new List<Word>();
        Name = name;
    }
    public MyDictionary(string name, List<Word> list)
    {
        if (list != null)
            words = list;
        else
            words = new List<Word>();

        Name = name;
    }
    public void Serialize(int index)
    {
        File.WriteAllText($"{Name}.json", JsonConvert.SerializeObject(words));
    }
    public int SearchWord(string word)
    {
        if (word.Length == 0)
            return -1;

        for (int i = 0; i < words.Count; i++)
        {
            if (words[i].OriginalWord == word)
                return i;
        }

        return -1;
    }

    public int SearchTranslate(string translate)
    {
        if (translate.Length == 0)
            return -1;

        for (int i = 0; i < words.Count; i++)
        {
            foreach (var item in words[i].translations)
            {
                if (item == translate)
                    return i;
            }
        }

        return -1;
    }

    public bool AddWord(string originalWord, string translate)
    {
        if (originalWord.Length == 0 || translate.Length == 0)
            return false;

        words.Add(new Word(originalWord, translate));
        return true;
    }

    public bool AddTranslate(string originalWord, string translate)
    {
        if (originalWord.Length == 0 || translate.Length == 0)
            return false;

        int index = SearchWord(originalWord);
        if (index < 0) return false;

        return words[index].AddTranslate(translate);
    }

    public bool ChangeWord(string old_originalWord, string new_originalWord)
    {
        if (old_originalWord.Length == 0 || new_originalWord.Length == 0)
            return false;

        int index = SearchWord(old_originalWord);
        if (index < 0) return false;

        words[index].OriginalWord = new_originalWord;
        return true;
    }

    public bool RemoveWord(string originalWord)
    {
        int index = SearchWord(originalWord);
        if (index < 0) return false;

        words.RemoveAt(index);
        return true;
    }

    public bool RemoveTranslate(string originalWord, string translate)
    {
        int index = SearchWord(originalWord);
        if (index < 0) return false;

        return words[index].RemoveTranslate(translate);
    }

    public Word GetWord(int index)
    {
        if (index < 0 || index >= words.Count)
            return new Word("0", "0");

        return words[index];
    }

    public bool PrintWords(string partOfWord)
    {
        bool result = false;
        foreach (var item in words)
        {
            if (item.OriginalWord.StartsWith(partOfWord))
            {
                item.ShowWord();
                result = true;
            }
        }

        return result;
    }
}

/*
Создать приложение «Словари».
Основная задача проекта: хранить словари на разных языках и разрешать пользователю находить перевод нужного слова или фразы.
Интерфейс приложения должен предоставлять такие возможности:
■ Создавать словарь. При создании нужно указать тип словаря.
Например, англо-русский или русско-английский.
■ Добавлять слово и его перевод в уже существующий словарь. Так как у слова может быть несколько переводов, необходимо поддерживать возможность создания 
нескольких вариантов перевода.
■ Заменять слово или его перевод в словаре.
■ Удалять слово или перевод. Если удаляется слово, все его переводы удаляются 
вместе с ним. Нельзя удалить перевод слова, если это последний вариант перевода.
■ Искать перевод слова.
■ Словари должны храниться в файлах.
■ Слово и варианты его переводов можно экспортировать в отдельный файл результата.
■ При старте программы необходимо показывать меню для работы с программой. 
Если выбор пункта меню открывает подменю, то тогда в нем требуется предусмотреть возможность возврата в предыдущее меню.
*/