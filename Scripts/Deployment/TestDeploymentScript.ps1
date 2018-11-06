Add-AzureRmAccount

Select-AzureRmSubscription -SubscriptionName "Azure for Students"

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
$template = -Join ($dir, "\ARMTemplate.json")
$resourceGroupName = Read-Host -Prompt 'Fill in the resource group the testdeployment should be run against: '

Test-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $template