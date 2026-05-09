_default:
  @just --list
  
run:
  dotnet run --project Web --launch-profile https
  
watch:
  dotnet watch run --quiet --project Web --launch-profile https
  
fmt:
  dotnet csharpier format .
  
upsubtree:
    git subtree pull --prefix=libs/KozLibraries git@github.com:koko-u/KozLibraries.git main --squash
    jj git import
    dotnet build
