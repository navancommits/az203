using System;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Compute.Fluent.Models;

namespace CreateVMUsingSubscription
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromFile("./azureauth.properties");
            var azure = Azure.Configure().WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic).Authenticate(credentials).WithDefaultSubscription();
            var groupName = "navaz203-resourcegrp";
            var location = Region.USCentral;
            var vNetName = "navaz203-vnetname";
            var vNetAddress = "172.16.0.0/16";
            var subNetName = "navaz203-subnetname";
            var subNetAddress = "172.16.0.0/24";
            var nicName = "navaz203-nicname";
            var vmName = "navaz203-vmname";
            var publicAddressName = "navaz203-pip";
            var resourceGroup = azure.ResourceGroups.Define(groupName).WithRegion(location).Create();
            var network = azure.Networks.Define(vNetName).WithRegion(location).WithExistingResourceGroup(resourceGroup).WithAddressSpace(vNetAddress).WithSubnet(subNetName, subNetAddress).Create();
            var pip = azure.PublicIPAddresses.Define(publicAddressName).WithRegion(location).WithExistingResourceGroup(resourceGroup).WithDynamicIP().Create();
            var nic = azure.NetworkInterfaces.Define(nicName).WithRegion(location).WithExistingResourceGroup(resourceGroup).WithExistingPrimaryNetwork(network).WithSubnet(subNetName).WithPrimaryPrivateIPAddressDynamic().WithExistingPrimaryPublicIPAddress(pip).Create();
            var vm = azure.VirtualMachines.Define(vmName).WithRegion(location).WithExistingResourceGroup(resourceGroup).WithExistingPrimaryNetworkInterface(nic).WithLatestWindowsImage("MicrosoftWindowsServer", "WindowsServer", "2016-Datacenter-smalldisk").WithAdminUsername("navadmin").WithAdminPassword("N@van123456").WithComputerName(vmName).WithSize(VirtualMachineSizeTypes.StandardB2ms).Create();
            Console.WriteLine("Hello World!");
        }
    }
}
