# PassTheWord

PassTheWord is een .NET console-applicatie voor het genereren van wachtwoorden met hoge entropie.

De originele codebase is gerefactord zodat de applicatie:

- leesbaarder is;
- beter gestructureerd is;
- robuuster is;
- makkelijker uitbreidbaar is;
- eenvoudiger testbaar is.

Naast de refactoring zijn twee uitbreidingen toegevoegd:

1. Ondersteuning voor meerdere alfabetten, inclusief Russisch-cyrillisch.
2. Ondersteuning voor externe wachtwoordvalidators via hashes.

---

# Doel van de refactoring

De originele implementatie bevatte veel logica in één grote methode. Hierdoor was de code:

- moeilijk leesbaar;
- lastig te onderhouden;
- lastig uit te breiden;
- moeilijk testbaar.

De refactoring verdeelt verantwoordelijkheden over meerdere klassen zodat iedere klasse één duidelijke taak heeft.

---

# Projectstructuur

```text
PassTheWord
│
├── Alphabets
├── Generation
├── Logging
├── Replacements
├── Validation
├── PasswordOptions.cs
├── PasswordService.cs
└── Program.cs
```

## Verantwoordelijkheden

| Bestand / Map | Verantwoordelijkheid |
|---|---|
| `Program.cs` | Startpunt van de applicatie |
| `PasswordService.cs` | Coördineert het volledige generatieproces |
| `PasswordOptions.cs` | Bevat alle configuratieopties |
| `Alphabets` | Ondersteuning voor meerdere alfabetten |
| `Generation` | Verschillende generatie-strategieën |
| `Validation` | Validatie en externe verificatie |
| `Replacements` | Optionele character replacements |
| `Logging` | Logging systeem met meerdere log levels |

---

# Gebruikte design patterns

## Strategy Pattern

Gebruikt in:

- `IPasswordGenerationStrategy`
- `RandomPasswordStrategy`
- `DictionaryPasswordStrategy`

Er zijn meerdere manieren om een wachtwoord te genereren. Daarom is gekozen voor het Strategy Pattern.

Hierdoor kunnen nieuwe generatie-algoritmes eenvoudig worden toegevoegd zonder bestaande code aan te passen.

Voorbeeld:

```csharp
IPasswordGenerationStrategy strategy =
    new RandomPasswordStrategy();
```

---

## Factory Pattern

Gebruikt in:

- `PasswordGenerationFactory`

De factory bepaalt welke generation strategy gebruikt moet worden.

Hierdoor hoeft `PasswordService` niet zelf te weten welke concrete implementatie nodig is.

Voorbeeld:

```csharp
if (options.UseDictionary)
    return new DictionaryPasswordStrategy();

return new RandomPasswordStrategy();
```

---

## Facade Pattern

Gebruikt in:

- `PasswordService`

`PasswordService` vormt een eenvoudige interface voor het volledige proces:

1. opties valideren;
2. juiste strategy selecteren;
3. wachtwoord genereren;
4. replacements toepassen;
5. externe validatie uitvoeren.

Daardoor blijft `Program.cs` klein en overzichtelijk.

---


## Adapter Pattern


Gebruikt in:

- `IExternalPasswordVerifier`
- `ExternalPasswordValidationService`

De opdracht vereist ondersteuning voor externe wachtwoordvalidatie.

Met `IExternalPasswordVerifier` kunnen externe diensten gekoppeld worden zonder dat de rest van de applicatie aangepast hoeft te worden.

Externe services ontvangen nooit het echte wachtwoord, maar alleen een hash van het wachtwoord.

---

# Externe wachtwoordvalidatie

De applicatie bevat een adapter voor de
Have I Been Pwned Pwned Passwords API.

Deze adapter gebruikt het k-Anonymity model:
alleen de eerste 5 karakters van een SHA1-hash
worden naar de API gestuurd.
Het volledige wachtwoord of de volledige hash
verlaat nooit de applicatie.

---

# Ondersteuning voor meerdere alfabetten

Nieuwe alfabetten kunnen eenvoudig worden toegevoegd door `IAlphabet` te implementeren.

Voorbeeld:

```csharp
public class CyrillicAlphabet : IAlphabet
{
    public string Name => "Cyrillic";

    public string UppercaseCharacters =>
        "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    public string LowercaseCharacters =>
        "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
}
```

In `Program.cs` kan het alfabet toegevoegd worden:

```csharp
Alphabets = new List<IAlphabet>
{
    new LatinAlphabet(),
    new CyrillicAlphabet()
};
```

Hiermee is ondersteuning voor het Russisch-cyrillische alfabet toegevoegd zoals gevraagd in de opdracht.

---



Externe validators moeten `IExternalPasswordVerifier` implementeren.

Nieuwe externe validators kunnen worden toegevoegd door een nieuwe klasse te maken die `IExternalPasswordVerifier` implementeert. Daarna kan deze runtime toegevoegd worden met `passwordService.AddExternalVerifier(...)`, zonder bestaande validatiecode aan te passen.

Voorbeeld:

```csharp
public class ExampleVerifier : IExternalPasswordVerifier
{
    public string Name => "Example verifier";

    public string HashAlgorithmName => "SHA256";

    public bool IsSafe(string passwordHash)
    {
        return true;
    }
}
```

Runtime toevoegen:

```csharp
passwordService.AddExternalVerifier(
    new ExampleVerifier()
);
```

## Werking

1. De verifier geeft aan welk hash-algoritme gebruikt moet worden.
2. PassTheWord berekent de hash van het wachtwoord.
3. Alleen de hash wordt doorgestuurd.
4. De externe verifier bepaalt of het wachtwoord veilig is.

---

# Logging

De applicatie bevat een eenvoudig logging-systeem.

## Onderdelen

- `ILogger`
- `ConsoleLogger`
- `LogLevel`

## Beschikbare log levels

| Level | Gedrag |
|---|---|
| `None` | Geen logging |
| `Warnings` | Alleen waarschuwingen |
| `All` | Alle informatie |

Voorbeeld:

```csharp
ILogger logger = new ConsoleLogger(LogLevel.All);
```

---

# Validatie

`PasswordOptionsValidator` controleert onder andere:

- minimale lengte;
- maximale lengte;
- verplichte character groups;
- surrogate characters;
- geldige alfabetten;
- geldige replacements.

Hierdoor wordt ongeldige configuratie vroegtijdig afgekeurd.

---

# Character replacements

`CharacterReplacementService` ondersteunt optionele replacements.

Voorbeeld:

```csharp
Replacements = new Dictionary<char, char>
{
    { 'o', '0' },
    { 'i', '1' },
    { 's', '$' }
};
```

De replacements worden willekeurig toegepast om variatie te behouden.

---


# Tests

De solution bevat een apart testproject:

```text
PassTheWord.Tests
```

De tests zijn geschreven met NUnit.

Voorbeeldtests:

- PasswordOptionsValidatorTests
- RandomPasswordStrategyTests
- DictionaryPasswordStrategyTests
- PasswordServiceTests
- ExternalPasswordValidationServiceTests
- ConsoleLoggerTests

Tests kunnen uitgevoerd worden met:

```bash
dotnet test
```

---

# Project uitvoeren

```bash
dotnet run --project PassTheWord
```

---

# Security-opmerking

De originele implementatie gebruikte `Span<char>` buffers.

In deze refactor is gekozen voor string-gebaseerde verwerking omdat de focus van de opdracht ligt op:

- structuur;
- design patterns;
- leesbaarheid;
- onderhoudbaarheid;
- uitbreidbaarheid.

In een productieomgeving zou extra aandacht nodig zijn voor het wissen van gevoelige data uit het geheugen.

---

# Conclusie

Door de refactoring is de applicatie:

- beter gestructureerd;
- beter uitbreidbaar;
- eenvoudiger testbaar;
- makkelijker onderhoudbaar;
- voorbereid op externe uitbreidingen;
- voorbereid op meerdere alfabetten.