using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text;

namespace SuneDoes.UI.Pages.LucidDreaming;

public partial class LucidDreamingGlimmerComponent
{
    private static CultureInfo _enUs = new CultureInfo("en-US");
    private static long _currentId = 0L;
    private static object _idLock = new { };
    private static long NextId()
    {
        lock (_idLock)
            return ++_currentId;
    }
    private long _id = NextId();
    private string _idString;
    private string _animationId;
    private MarkupString? _cssAnimationRule;
    private MarkupString? _cssRule;
    private string _imageFile = $"images/lucid-dreaming/glimmer-{Random.Next(0,3)}.webp";

    public LucidDreamingGlimmerComponent()
    {
        _idString = $"sundo-lucing-dreaming-glimmer-{_id}";
        _animationId = $"{_idString}-animate";
    }


    [Parameter]
    public int NumberOfSeconds { get; set; }

    [Parameter]
    public int InitialDelay { get; set; }

    [Parameter]
    public int PercentLeft { get; set; }

    protected override Task OnInitializedAsync()
    {
        _cssRule = GenerateCssRule();
        _cssAnimationRule = GenerateAnimationRule();
        InvokeAsync(StateHasChanged);
        return base.OnInitializedAsync();
    }



    private static readonly Random Random = new Random(1);

    private const int MaxBrightness = 1000;
    private const int MinBrightness = 90;
    private static int DrawBrightness => Random.Next(MinBrightness, MaxBrightness);

    private const int MaxTranslate = 10;
    private const int MaxRotate = 30;

    private const int MinZindex = 99;
    private const int MaxZindex = 110;
    private int _zIndex = Random.Next(MinZindex, MaxZindex);


    private const int NumberOfKeyFrames = 20;


    private MarkupString GenerateCssRule()
    {
        var percentLeftToUse = PercentLeft;
        if (percentLeftToUse < 4)
            percentLeftToUse += Random.Next(2, 5);
        else if(percentLeftToUse > 96)
            percentLeftToUse -= Random.Next(10, 15);

        var returnee = new StringBuilder($"#{_idString} {{");
        returnee.AppendLine($"  opacity: 0;");
        returnee.AppendLine($"  position: absolute;");
        returnee.AppendLine($"  width: 10px;");
        returnee.AppendLine($"  height: 10px;");
        returnee.AppendLine($"  left: {percentLeftToUse}vw;");
        returnee.AppendLine($"  top: 0vh;");
        returnee.AppendLine($"  background-image: url('{_imageFile}');");
        returnee.AppendLine($"  animation: {_animationId} {NumberOfSeconds}s ease-in {InitialDelay}s 1;");
        returnee.AppendLine("}");
        return new MarkupString( returnee.ToString() );
    }

        private MarkupString GenerateAnimationRule()
    {
        var returnee = new StringBuilder($"@keyframes {_animationId} {{");
        var stepBy = 100 / NumberOfKeyFrames;
        var currentTranslate = 0;
        var currentRotate = 0;
        for(int perc = 0; perc <= 100; perc += stepBy)
        {
            var translateSign = currentTranslate < 0 ? 1 : -1;
            currentTranslate += Random.Next(0, 5) * translateSign;
            if (currentTranslate < -MaxTranslate)
                currentTranslate = -MaxTranslate;
            else if(currentTranslate > MaxTranslate) 
                currentTranslate = MaxTranslate;
            currentRotate += Random.Next(0, MaxRotate);
            var currentBrightness = DrawBrightness;
            var currentScale = 1.0 + Random.NextDouble() * 0.2;
            returnee.AppendLine($" {perc}% {{");
                        if(perc <= 10)
                            returnee.AppendLine($"    opacity: {(perc/10d).ToString("f1", _enUs)};");
            //returnee.AppendLine($"    opacity: 1;");
            returnee.AppendLine($"    filter: brightness({currentBrightness}%);");
            returnee.AppendLine($"    transform: rotate({currentRotate}deg) translate({currentTranslate}px) scale({currentScale.ToString("f5",_enUs)});");
            if (perc >= 90)
                returnee.AppendLine($"    opacity: {((100 - perc) / 10d).ToString("f1", _enUs)};");

            if (perc == 100)
               returnee.AppendLine($"    top: 100vh;");
            returnee.AppendLine(" }");
        }

        returnee.AppendLine("}");
        return new MarkupString(returnee.ToString());
    }



}
