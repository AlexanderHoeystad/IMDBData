// See https://aka.ms/new-console-template for more information

using IMDBData;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

int LineCount = 0;
List<Title> titles = new List<Title>();
string filePath = "C:/IMDBData/title.basics.tsv";
foreach (string line in File.ReadLines(filePath))
{
    //Console.WriteLine(line);

    if (LineCount == 50000)
    {
        break;
    }

    string[] splitLine = line.Split("\t");
    if (splitLine.Length != 9)
    {
        throw new Exception("Incorrect number of tabs! " + line);
    }

    string tconst = splitLine[0];
    string titleType = splitLine[1];
    string primaryTitle = splitLine[2];
    string originalTitle = splitLine[3];
    // Kan den måske være andet end 1 og 0 ?????????
    bool isAdult = splitLine[4] == "1";
    int? startYear = ParseInt(splitLine[5]);
    int? endYear = ParseInt(splitLine[6]);
    int? runtimeMinutes = ParseInt(splitLine[7]);

    Title newTitle = new Title()
    {
        Tconst = tconst,
        TitleType = titleType,
        PrimaryTitle = primaryTitle,
        OriginalTitle = originalTitle,
        IsAdult = isAdult,
        StartYear = startYear,
        EndYear = endYear,
        RuntimeMinutes = runtimeMinutes
    };

    titles.Add(newTitle);


    LineCount++;  // Move this outside of the if block, to correctly increment on each line
}

Console.WriteLine("list of titles length" + titles.Count);

SqlConnection sqlConnect = new SqlConnection("server=localhost;database=IMDBDatabase;" + "user id=sa;password=GruppeLilla5; TrustServerCertificate=True");

sqlConnect.Open();
SqlTransaction transaction = sqlConnect.BeginTransaction();

DateTime before = DateTime.Now;

try
{
    NormalInserter inserter = new NormalInserter();
    inserter.InsertData(titles, sqlConnect, transaction);
    //transaction.Commit();
    transaction.Rollback();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    transaction.Rollback();
}


DateTime after = DateTime.Now;

sqlConnect.Close();

Console.WriteLine("Time taken: " + (after - before));

int? ParseInt(string value)
{
    // Check if the value is "\N" (used to represent NULL in many TSV files) or an empty string
    if (string.IsNullOrWhiteSpace(value) || value == "\\N")
    {
        return null;
    }

    // Try to parse the string into an integer
    if (int.TryParse(value, out int result))
    {
        return result;
    }

    // If parsing fails, return null
    return null;
}


