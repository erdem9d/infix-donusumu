using System;
using System.Collections.Generic;

// Bağlı liste düğümünü tanımlayan sınıf
class LinkedListNode
{
    public string Value; // Düğümün içindeki değer
    public LinkedListNode Next; // Bir sonraki düğümü işaret eder

    public LinkedListNode(string value)
    {
        Value = value; // Düğümün değerini ayarlıyoruz
        Next = null; // Başlangıçta sonraki düğüm yok
    }
}

// Bağlı listeyi tanımlayan sınıf
class LinkedList
{
    private LinkedListNode head; // Listenin ilk düğümü

    // Listeye yeni bir düğüm ekleyen metot
    public void Add(string value)
    {
        var newNode = new LinkedListNode(value); // Yeni bir düğüm oluşturuyoruz
        if (head == null)
        {
            head = newNode; // Eğer liste boşsa, yeni düğüm baş düğüm oluyor
        }
        else
        {
            var current = head; // Mevcut düğümü başa atıyoruz
            while (current.Next != null) // Listenin sonuna kadar gidiyoruz
            {
                current = current.Next; // Bir sonraki düğüme geçiyoruz
            }
            current.Next = newNode; // Yeni düğümü listenin sonuna ekliyoruz
        }
    }

    // Listedeki tüm düğümleri yazdıran metot
    public void Print()
    {
        var current = head; // Mevcut düğümü başa atıyoruz
        while (current != null) // Tüm düğümleri gezerek
        {
            Console.Write(current.Value + " "); // Düğüm değerini ekrana yazdırıyoruz
            current = current.Next; // Bir sonraki düğüme geçiyoruz
        }
        Console.WriteLine(); // Yazdırma işlemini bitiriyoruz
    }

    // Listeyi diziye dönüştüren metot
    public string[] ToArray()
    {
        List<string> items = new List<string>(); // Boş bir liste oluşturuyoruz
        var current = head; // Mevcut düğümü başa atıyoruz
        while (current != null) // Tüm düğümleri gezerek
        {
            items.Add(current.Value); // Düğüm değerini diziye ekliyoruz
            current = current.Next; // Bir sonraki düğüme geçiyoruz
        }
        return items.ToArray(); // Listeyi diziye çevirip döndürüyoruz
    }
}

class Program
{
    // Operatörlerin önceliğini belirleyen metot
    static int GetPrecedence(char op)
    {
        switch (op)
        {
            case '+':
            case '-':
                return 1; // Toplama ve çıkarma için öncelik 1
            case '*':
            case '/':
                return 2; // Çarpma ve bölme için öncelik 2
            case '^':
                return 3; // Üst alma için en yüksek öncelik 3
            default:
                return 0; // Diğer durumlar için öncelik 0
        }
    }

    // Karakterin bir operatör olup olmadığını kontrol eden metot
    static bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
    }

    // Infix ifadesini Postfix'e dönüştüren metot
    static LinkedList InfixToPostfix(string infix)
    {
        LinkedList postfix = new LinkedList(); // Postfix için yeni bir liste oluşturuyoruz
        Stack<char> stack = new Stack<char>(); // Operatörleri saklamak için bir yığın oluşturuyoruz

        foreach (char token in infix) // Infix ifadedeki her karakter için döngü
        {
            if (char.IsDigit(token) || char.IsLetter(token)) // Eğer karakter bir sayı veya harfse
            {
                postfix.Add(token.ToString()); // Postfix listesine ekliyoruz
            }
            else if (IsOperator(token)) // Eğer karakter bir operatörse
            {
                while (stack.Count > 0 && GetPrecedence(stack.Peek()) >= GetPrecedence(token)) // Yığındaki operatörlerin önceliklerini karşılaştırıyoruz
                {
                    postfix.Add(stack.Pop().ToString()); // Yığından operatörü çıkarıp postfix listesine ekliyoruz
                }
                stack.Push(token); // Yeni operatörü yığına ekliyoruz
            }
        }

        while (stack.Count > 0) // Yığın boşalana kadar
        {
            postfix.Add(stack.Pop().ToString()); // Yığındaki tüm operatörleri çıkarıp postfix listesine ekliyoruz
        }

        return postfix; // Postfix ifadesini döndürüyoruz
    }

    // Postfix ifadesini değerlendiren metot
    static double EvaluatePostfix(LinkedList postfix)
    {
        Stack<double> stack = new Stack<double>(); // Sayıları saklamak için bir yığın oluşturuyoruz
        string[] tokens = postfix.ToArray(); // Postfix ifadeyi diziye çeviriyoruz

        foreach (string token in tokens) // Her token için döngü
        {
            if (double.TryParse(token, out double number)) // Eğer token sayıya dönüştürülebiliyorsa
            {
                stack.Push(number); // Sayıyı yığına ekliyoruz
            }
            else // Aksi halde bir operatördür
            {
                double right = stack.Pop(); // Sağdaki değeri alıyoruz
                double left = stack.Pop(); // Soldaki değeri alıyoruz
                switch (token[0]) // Operatöre göre işlemi yapıyoruz
                {
                    case '+':
                        stack.Push(left + right); // Toplama işlemi
                        break;
                    case '-':
                        stack.Push(left - right); // Çıkarma işlemi
                        break;
                    case '*':
                        stack.Push(left * right); // Çarpma işlemi
                        break;
                    case '/':
                        stack.Push(left / right); // Bölme işlemi
                        break;
                    case '^':
                        stack.Push(Math.Pow(left, right)); // Üst alma işlemi
                        break;
                }
            }
        }

        return stack.Pop(); // Sonucu döndürüyoruz
    }

    // Postfix ifadesini Prefix'e dönüştüren metot
    static LinkedList PostfixToPrefix(LinkedList postfix)
    {
        Stack<string> stack = new Stack<string>(); // Dönüşüm için bir yığın oluşturuyoruz
        string[] tokens = postfix.ToArray(); // Postfix ifadeyi diziye çeviriyoruz

        for (int i = 0; i < tokens.Length; i++) // Her token için döngü
        {
            if (double.TryParse(tokens[i], out double number)) // Eğer token sayıya dönüştürülebiliyorsa
            {
                stack.Push(tokens[i]); // Sayıyı yığına ekliyoruz
            }
            else // Aksi halde bir operatördür
            {
                string right = stack.Pop(); // Sağdaki ifadeyi alıyoruz
                string left = stack.Pop(); // Soldaki ifadeyi alıyoruz
                string exp = tokens[i] + left + right; // Prefix ifadesini oluşturuyoruz
                stack.Push(exp); // Oluşan ifadeyi yığına ekliyoruz
            }
        }

        LinkedList prefix = new LinkedList(); // Yeni bir prefix listesi oluşturuyoruz
        prefix.Add(stack.Pop()); // Yığındaki son ifadeyi prefix listesine ekliyoruz
        return prefix; // Prefix ifadesini döndürüyoruz
    }

    // Prefix ifadesini değerlendiren metot
    static double EvaluatePrefix(LinkedList prefix)
    {
        Stack<double> stack = new Stack<double>(); // Sayıları saklamak için bir yığın oluşturuyoruz
        string[] tokens = prefix.ToArray(); // Prefix ifadeyi diziye çeviriyoruz
        Array.Reverse(tokens); // Prefix ifadeyi ters çeviriyoruz

        foreach (string token in tokens) // Her token için döngü
        {
            if (double.TryParse(token, out double number)) // Eğer token sayıya dönüştürülebiliyorsa
            {
                stack.Push(number); // Sayıyı yığına ekliyoruz
            }
            else // Aksi halde bir operatördür
            {
                double left = stack.Pop(); // Soldaki değeri alıyoruz
                double right = stack.Pop(); // Sağdaki değeri alıyoruz
                switch (token[0]) // Operatöre göre işlemi yapıyoruz
                {
                    case '+':
                        stack.Push(left + right); // Toplama işlemi
                        break;
                    case '-':
                        stack.Push(left - right); // Çıkarma işlemi
                        break;
                    case '*':
                        stack.Push(left * right); // Çarpma işlemi
                        break;
                    case '/':
                        stack.Push(left / right); // Bölme işlemi
                        break;
                    case '^':
                        stack.Push(Math.Pow(left, right)); // Üst alma işlemi
                        break;
                }
            }
        }

        return stack.Pop(); // Sonucu döndürüyoruz
    }
}

    
