# Suggestions in .NET MAUI AI AssistView

This sample demonstrates how to integrate Suggestions into the .NET MAUI AI AssistView control within a .NET MAUI application.

## Sample

```xaml
    <assistview:SfAIAssistView x:Name="sfAIAssistView" ShowHeader="True"
                               HeaderTemplate="{StaticResource headerTemplate}"
                               Suggestions="{Binding Suggestions}"
                               SuggestionTemplate="{StaticResource suggestionTemplate}"
                               AssistItems="{Binding Messages}"
                               RequestCommand="{Binding AssistViewRequestCommand}"/>
```

## Requirements to run the demo

To run the demo, refer to [System Requirements for .NET MAUI](https://help.syncfusion.com/maui/system-requirements)

## Troubleshooting:

### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.

## License

Syncfusion has no liability for any damage or consequence that may arise from using or viewing the samples. The samples are for demonstrative purposes. If you choose to use or access the samples, you agree to not hold Syncfusion liable, in any form, for any damage related to use, for accessing, or viewing the samples. By accessing, viewing, or seeing the samples, you acknowledge and agree Syncfusion's samples will not allow you seek injunctive relief in any form for any claim related to the sample. If you do not agree to this, do not view, access, utilize, or otherwise do anything with Syncfusion's samples.