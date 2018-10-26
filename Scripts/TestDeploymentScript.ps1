Add-AzureRmAccount

Select-AzureRmSubscription -SubscriptionName "Azure for Students"

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
$template = -Join ($dir, "\ARMTemplate.json")

Test-AzureRmResourceGroupDeployment -ResourceGroupName ELBHO-API -TemplateFile $template