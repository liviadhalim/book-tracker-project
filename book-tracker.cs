//Livia Halim - BIT 143 - 2015 - Fall - A3.1
using System;
using System.Collections.Generic;
using System.Text;

namespace MulitList_Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            (new UserInterface()).RunProgram();

            // Or, you could go with the more traditional:
            // UserInterface ui = new UserInterface();
            // ui.RunProgram();
        }
    }

    // Bit of a hack, but still an interesting idea....
    enum MenuOptions
    {
        // DO NOT USE ZERO!
        // (TryParse will set choice to zero if a non-number string is typed,
        // and we don't want to accidentally set nChoice to be a member of this enum!)
        QUIT = 1,
        ADD_BOOK,
        PRINT_BY_AUTHOR,
        PRINT_BY_TITLE,
        REMOVE_BOOK,
        RUN_TESTS
    }

    class UserInterface
    {
        MultiLinkedListOfBooks theList;
        public void RunProgram()
        {
            int nChoice;
            theList = new MultiLinkedListOfBooks();

            do // main loop
            {
                Console.WriteLine("Your options:");
                Console.WriteLine("{0} : End the program", (int)MenuOptions.QUIT);
                Console.WriteLine("{0} : Add a book", (int)MenuOptions.ADD_BOOK);
                Console.WriteLine("{0} : Print all books (by author)", (int)MenuOptions.PRINT_BY_AUTHOR);
                Console.WriteLine("{0} : Print all books (by title)", (int)MenuOptions.PRINT_BY_TITLE);
                Console.WriteLine("{0} : Remove a Book", (int)MenuOptions.REMOVE_BOOK);
                Console.WriteLine("{0} : RUN TESTS", (int)MenuOptions.RUN_TESTS);
                if (!Int32.TryParse(Console.ReadLine(), out nChoice))
                {
                    Console.WriteLine("You need to type in a valid, whole number!");
                    continue;
                }
                switch ((MenuOptions)nChoice)
                {
                    case MenuOptions.QUIT:
                        Console.WriteLine("Thank you for using the multi-list program!");
                        break;
                    case MenuOptions.ADD_BOOK:
                        this.AddBook();
                        break;
                    case MenuOptions.PRINT_BY_AUTHOR:
                        theList.PrintByAuthor();
                        break;
                    case MenuOptions.PRINT_BY_TITLE:
                        theList.PrintByTitle();
                        break;
                    case MenuOptions.REMOVE_BOOK:
                        this.RemoveBook();
                        break;
                    case MenuOptions.RUN_TESTS:
                        AllTests tester = new AllTests();
                        tester.RunTests();
                        break;
                    default:
                        Console.WriteLine("I'm sorry, but that wasn't a valid menu option");
                        break;

                }
            } while (nChoice != (int)MenuOptions.QUIT);
        }

        public void AddBook()
        {
            Console.WriteLine("ADD A BOOK!");

            Console.WriteLine("Author name?");
            string author = Console.ReadLine();

            Console.WriteLine("Title?");
            string title = Console.ReadLine();

            double price = -1;
            while (price < 0)
            {
                Console.WriteLine("Price?");
                if (!Double.TryParse(Console.ReadLine(), out price))
                {
                    Console.WriteLine("I'm sorry, but that's not a number!");
                    price = -1;
                }
                else if (price < 0)
                {
                    Console.WriteLine("I'm sorry, but the number must be zero, or greater!!");
                }
            }

            //if ec is Duplicate Book, print the message to the user
            //if ec is OK, print the message to the user

            ErrorCode ec = theList.Add(author, title, price);

            if (ec == ErrorCode.DuplicateBook)
            {
                Console.WriteLine("Book is a duplicate!");
            }
            else if (ec == ErrorCode.OK)
            {
                Console.WriteLine("Book was added!");
            }

        }

        public void RemoveBook()
        {
            Console.WriteLine("REMOVE A BOOK!");

            Console.WriteLine("Author name?");
            string author = Console.ReadLine();

            Console.WriteLine("Title?");
            string title = Console.ReadLine();

            ErrorCode ec = theList.Remove(author, title);

            //if ec is BookNotFound, print the message to the user
            //if ec is OK, print the message to the user

            if (ec == ErrorCode.BookNotFound)
            {
                Console.WriteLine("Book is not found!");

            }
            else if (ec == ErrorCode.OK)
            {
                Console.WriteLine("Book was removed!");
            }
        }
    }

    enum ErrorCode
    {
        OK,
        DuplicateBook,
        BookNotFound
    }

    class MultiLinkedListOfBooks
    {
        Book removed; //book object so we don't have to create multiple objects whenever we remove a book

        private class Book
        {

            //creating two pointers
            public Book nextAuthor;
            public Book nextTitle;
            //creating variables for book
            string title;
            string author;
            double price;

            //creating two kinds of constructor
            public Book(string a, string t)
            {
                author = a;
                title = t;
            }
            public Book(string a, string t, double p)
            {
                title = t;
                author = a;
                price = p;
            }
            //making a print method
            public void Print()
            {
                Console.WriteLine("Book Title:{0}", title);
                Console.WriteLine("Book Author:{0}", author);
                Console.WriteLine("Price:{0}", price);
            }
            //two methods that compare author and title, and will return either -1 (before), 0 (the same), 1 (after)
            public int CompareByAuthor(Book otherBook)
            {
                return this.author.CompareTo(otherBook.author);
            }
            public int CompareByTitle(Book otherBook)
            {
                return this.title.CompareTo(otherBook.title);
            }
            //method that returns either -1 (before), 0 (the same), 1 (after) for the title, if the author is the same
            //method that returns either -1 (before), 0 (the same), 1 (after) for the author, if the title is the same
            public int CompareAuthorFirst(Book otherBook)
            {
                if (this.CompareByAuthor(otherBook) == 0)
                {
                    return CompareByTitle(otherBook);
                }
                else
                    return Int32.MinValue;
            }

            public int CompareTitleFirst(Book otherBook)
            {
                if (this.CompareByTitle(otherBook) == 0)
                {
                    return CompareByAuthor(otherBook);
                }
                else
                    return Int32.MinValue;
            }

        }
        //make two book objects for the first one in the author and the first one in the title
        private Book firstA;
        private Book firstT;
        public ErrorCode Add(string author, string title, double price)
        {
            Book newbook; //making book object to store new books
            
            if (firstA == null && firstT == null)
            {
                newbook = new Book(author, title, price);
                firstA = newbook;
                firstT = newbook;
                return ErrorCode.OK;
            }
            else
            {
                newbook = new Book(author, title, price);
                //making two book objects as holders
                Book curT = firstT;
                Book curA = firstA;
                //inserting a book at front of the first one in the title pointer
                if (newbook.CompareByTitle(firstT) == -1)
                {
                    newbook.nextTitle = firstT;
                    firstT = newbook;
                }
                //what to do if the new book's title is the same as the first one in the title pointer
                else if (newbook.CompareByTitle(firstT) == 0)
                {
                    if (newbook.CompareByAuthor(firstT) == -1)
                    {
                        newbook.nextTitle = firstT;
                        firstT = newbook;
                    }
                    //return errorcode of duplicate if the new book's title and author are the same as the title and author of first one in title pointer
                    if (newbook.CompareByAuthor(firstT) == 0)
                    {
                        return ErrorCode.DuplicateBook;
                    }
                    //going through a loop if the new book's author should be put after the first one in the title pointer's author
                    if (newbook.CompareByAuthor(firstT) == 1)
                    {
                        while (newbook.CompareByAuthor(curT) != 1)
                        {
                            curT = curT.nextTitle;
                        }
                        newbook.nextTitle = curT.nextTitle;
                        curT.nextTitle = newbook;
                    }

                }
                //what to do if the new book should be put after the first one in the title pointer
                else if (newbook.CompareByTitle(firstT) == 1)
                {
                    while (curT != null || newbook.CompareByTitle(curT) != 1) //do a loop to tell where to put
                    {
                        if (curT.nextTitle == null)
                        {
                            curT.nextTitle = newbook;
                            break;
                        }
                        curT = curT.nextTitle;
                        if (newbook.CompareByTitle(curT) == -1)
                        {
                            newbook.nextTitle = curT.nextTitle;
                            curT.nextTitle = newbook;
                            break;
                        }
                        if (newbook.CompareByTitle(curT) == 0)
                        {
                            if (newbook.CompareByAuthor(curT) == -1)
                            {
                                newbook.nextTitle = curT.nextTitle;
                                curT.nextTitle = newbook;
                                break;
                            }
                            if(newbook.CompareByAuthor(curT) ==0)
                            {
                                return ErrorCode.DuplicateBook;
                            }
                            else if (newbook.CompareByAuthor(curT) == 1)
                            {
                                while (newbook.CompareByAuthor(curT) != 1)
                                {
                                    curT = curT.nextTitle;
                                }
                                newbook.nextTitle = curT.nextTitle;
                                curT.nextTitle = newbook;
                                break;
                            }

                        }

                    }

                }

                //do the same thing as above for author pointer
                if (newbook.CompareByAuthor(firstA) == -1)
                {
                    newbook.nextAuthor = firstA;
                    firstA = newbook;
                    return ErrorCode.OK;
                }
                else if (newbook.CompareByAuthor(firstA) == 0)
                {
                    if (newbook.CompareByTitle(firstA) == -1)
                    {
                        newbook.nextAuthor = firstA;
                        firstA = newbook;
                        return ErrorCode.OK;
                    }
                    if (newbook.CompareByTitle(firstA) == 1)
                    {
                        while(newbook.CompareByTitle(curA) != 1)
                        {
                            curA= curA.nextAuthor;
                        }
                        newbook.nextAuthor = curA.nextAuthor;
                        curA.nextAuthor = newbook;
                        return ErrorCode.OK;
                    }

                }
                else if (newbook.CompareByAuthor(firstA) == 1)
                {

                    while (curA != null || newbook.CompareByAuthor(curA) !=1)
                    {
                       
                        if (curA.nextAuthor == null)
                        {
                            curA.nextAuthor = newbook;
                            return ErrorCode.OK;
                        }
                        curA = curA.nextAuthor;
                        if (newbook.CompareByAuthor(curA) == -1)
                        {
                            newbook.nextAuthor = curA.nextAuthor;
                            curA.nextAuthor = newbook;
                            return ErrorCode.OK;
                        }

                        if (newbook.CompareByAuthor(curA) == 0)
                        {
                            if (newbook.CompareByTitle(curA) == -1)
                            {
                                newbook.nextAuthor = curA.nextAuthor;
                                curA.nextAuthor= newbook;
                                return ErrorCode.OK;
                            }
                            else if (newbook.CompareByTitle(curA) == 1)
                            {
                                while (newbook.CompareByTitle(curA) != 1)
                                {
                                    curA = curA.nextAuthor;
                                }
                                newbook.nextAuthor = curA.nextAuthor;
                                curA.nextAuthor = newbook;
                                return ErrorCode.OK;
                            }

                        }

                    }

                }



            }
            return ErrorCode.DuplicateBook;
        }

        public void PrintByAuthor()
        {
            // if there are no books, then print out a message saying that the list is empty
            Book curA = firstA;
            if (curA == null)
                Console.WriteLine("List is empty!");
            //go through each one in the list to print
            while (curA != null)
            {
                curA.Print();
                curA = curA.nextAuthor;
            }
        }
        public void PrintByTitle()
        {
            // if there are no books, then print out a message saying that the list is empty
            Book curT = firstT;
            if (curT == null)
                Console.WriteLine("List is empty!");
            //go through each one in the list to print
            while (curT != null)
            {
                curT.Print();
                curT = curT.nextTitle;
            }
        }

        public ErrorCode Remove(string author, string title)
        {
            removed = new Book(author, title);
            
            if (firstA == null & firstT == null)
                return ErrorCode.BookNotFound; // if there is nothing in the list, return the errorcode
            else
            {
                Book curA = firstA;
                Book curT = firstT;
                //check if the first one in the author pointer is the same, and so is the title. 
                //if so, remove.
                if (removed.CompareAuthorFirst(firstA) == 0)
                {
                    firstA = firstA.nextAuthor;
                }
                else
                {
                    //check if the book is in the next ones in the author pointer
                    while (curA != null)
                    {
                        
                        if (removed.CompareAuthorFirst(curA.nextAuthor) == 0)
                        {
                            curA.nextAuthor = curA.nextAuthor.nextAuthor;
                            break;
                        }
                        else
                            curA = curA.nextAuthor;
                        if(curA == null) //if we have gone to null without breaking, return the error code
                        {
                            return ErrorCode.BookNotFound;
                        }
                    }
                }

                //do the same thing with the title pointer
                if (removed.CompareTitleFirst(firstT) == 0)
                {
                    firstT = firstT.nextTitle;
                    return ErrorCode.OK; //return errorcode OK after removing from the title pointer too 
                }
                while (curT != null)
                {
                    if (removed.CompareTitleFirst(curT.nextTitle) == 0)
                    {
                        curT.nextTitle = curT.nextTitle.nextTitle;
                        return ErrorCode.OK; //return errorcode OK after removing from the title pointer too 
                    }
                    else
                        curT = curT.nextTitle;

                }
                
            }
            return ErrorCode.BookNotFound;
        }

    }



    class AllTests
    {
        public void RunTests()
        {
        }
    }
}
