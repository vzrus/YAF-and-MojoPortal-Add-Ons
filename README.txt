To use this you should compile MP solution with extra projects added.


1.Prepare YAF source code.

1.Compiling YAF.

YetAnotherForum.NET - is your future distribution folder which should be renamed to YAF by default settings.
But you should replace YAF.Providers foldercontent with the one on the site, so you should compile it anyway. 

Otherwise rename folder were are distribution files.
Replace scripts in the distribution by FileToReplaceInYAFFiles folder content in the YetAnotherForum.Mojo.Src files folder.

Now can compile it and make sure that it works or take a ready code beginning with 1.9.6 Beta 1(currently).


*It contains scripts for YAF.Providers project and a respective dll. This is modifies YAF Providers without which your integreatin will not be full. Alteranatively you can use your own Profile provider in the assembly, the main condition - it should beb linked to a working Membership for mojoPortal or to be fully independent.

2. After you compiled YAF and are sure that everything works.
 
2.1. Put the YAF folder into MP 'Web' folder. Don't add it to MP solution!

2.2. Put YetAnotherForum.Mojo.Src folder into MP main solution folder.

3. Merge configs - look for example working config in the folder (Web.sample.config). Just replace you connection strings. For further details go to MP site. Fix merged web.config
Set this value in web.config  
 <add key="YAF.ConfigPassword" value="yourpass" /> It will be required to enter while you install YAF.
Change connection strings (1 for YAF and the second for MP)
Don't use user.config in the folder - it's for reference only.

4.Remove old YAF Web.config, db.config and app.config from the folder. Move all assemblies from bin folder to MP bin folder and remove YAF bin folder and other things to not publish unused files.

5. Fix assemblies references of your projects in MP solution - they should point to assemblies in MP bin folder. Compile it.


6. Before install and module install.

YAF stores passwords encripted - make it for MP admin too before install just in case.

Now you can simply add the module to MP as usual module, when you first time go to module setting it'll redirect you to YAF install page. You should install YAF. 

Be careful with Admin name which you shoud enter while installing. You should remember you MP host admin email which should be an existing MP admin name.
Use existing user name when required. Like admin@admin.com, don't create a new admin user.
After install you login under the name in MP again. Now you're YAF host admin.

7.Post install actions.

Click on Host on the upper panel. Check Enable Display Name: on Features tab.

8.In the left accordian menu find User and Role -> Roles setting. Assign required privrleges.

9. Ready.

Warning! Don't put 2 YAF modules on a page. It's useless.
Don't use 2 modules and many YAF boards so far - it's not tested well so far.

    
