$kvName = 'testkv20200617'
$rgName = 'testresgrp20200617'
$location = 'West US'
$appDisplayName = 'test-aad-app-20200617'
$keyEncryptionKeyName='testenckey20200617'

New-AzureRmResourceGroup -Name $rgName -Location $location
New-AzureRmKeyVault -VaultName $kvName -ResourceGroupName $rgName -Location $location

Set-AzureRmKeyVaultAccessPolicy -VaultName $kvName -ResourceGroupName $rgName -EnabledForDiskEncryption

$aadApp = New-AzureRmADApplication -DisplayName $appDisplayName -HomePage 'http://homepagetestEncryptApp' -IdentifierUris 'http://uritestEncryptApp' 

$appID = $aadApp.ApplicationId

$aadServicePrincipal = New-AzureRmADServicePrincipal -ApplicationId $appID

Set-AzureRmKeyVaultAccessPolicy -VaultName $kvName -ServicePrincipalName $appID -PermissionsToKeys get,list,update,create,import,delete -PermissionsToSecrets get,list,set,delete,recover,backup,restore

Add-AzureKeyVaultKey -VaultName $kvName -Name $keyEncryptionKeyName -Destination 'Software'

$keyEncryptionKeyUrl=(Get-AzureKeyVaultKey -VaultName $kvName -Name $keyEncryptionKeyName).Key.kid

$keyVault=Get-AzureRMKeyVault -VaultName $kvName

Set-AzureRmVMDiskEncryptionExtension -ResourceGroupName $rgName -VMName 'mynewvm20200617' -DiskEncryptionKeyVaultUrl $keyVault.VaultUri -DiskEncryptionKeyVaultId $keyVault.ResourceId -KeyEncryptionKeyUrl $keyEncryptionKeyUrl -KeyEncryptionKeyVaultId $keyVault.ResourceId
