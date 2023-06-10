# Enterprise IP Exchange C# SDK
## ws3dx.exchange

This ws3dx.exchange demo exercises an C# SDK that accesses the 3DEXPERIENCE Enterprise IP Exchange web services. The current version focuses only on basic export functionality,

- CreateExportJob
- SubmitExportJob
- GetExportJob

## CreateExportJob
- Exercises the creation of an Export Job.
```csharp

	TypedUriIdentifier id = new TypedUriIdentifier();
	id.Type = "VPMReference";
	id.Source = m_serviceUrl;
	id.RelativePath = $"/resource/v1/dseng/VPMReference/{rootEngItemId}";
	id.Identifier = rootEngItemId;

	EngineeringDomainExportParameter engExportParam = new EngineeringDomainExportParameter(); ;
	engExportParam.ExpandDepth = "0"; //-1 expand everything

	NewExportJob newExportJob = new NewExportJob();
	newExportJob.Title = "ERP Export test";

	newExportJob.Objects = new List<object> {
		id
	};

	 newExportJob.Parameters = new List<object> {
		engExportParam
	 };

     ExportJobRef? newJobRef = await service.CreateExportJob(newExportJob);
	 
```
## SubmitExportJob
- Submits a previously created export job (see previous step)
```csharp

	if (!await service.SubmitExportJob(newJobRef.Identifier))
	{
		//TODO: Handle error
		return;
	}
	 
```

## GetExportJob
- Gets the status of a previously submitted job (see previous step)
```csharp

	DateTime pollingStart = DateTime.Now;

	ExportJob exportJob = null;

	do
	{

		if (DateTime.Now > pollingStart.AddSeconds(timeoutSecs))
		{
		   timeOutRaised = true;
		   break;
		}

		Thread.Sleep(pollingIntervalSecs * 1000);

		exportJob = await service.GetExportJob(newJobRef.Identifier);

	}
	while ((exportJob != null) && (!exportJob.HasCompleted));
	 
```

## License

MIT

## Disclaimer

This is only a demo product and _it is NOT an official Dassault Systèmes product_. It aims to demonstrate a possible way to build an SDK for C# that accesses the 3DEXPERIENCE web services. This demo is provided under the MIT license and any partner can take its source code and extend it to their own products always within the boundaries and constraints of the given license.
