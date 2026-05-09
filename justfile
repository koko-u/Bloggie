_default:
  @just --list
  
run:
  dotnet run --project Web --launch-profile https
  
watch:
  dotnet watch run --quiet --project Web --launch-profile https
  
fmt:
  dotnet csharpier format .
  
upsubtree:
    @if test -n "$(jj diff --stat)"; then \
        echo "Working copy has changes. Commit or move them before updating subtree."; \
        exit 1; \
    fi
    jj git export
    git subtree pull --prefix=libs/KozLibraries git@github.com:koko-u/KozLibraries.git main --squash
    jj git import
    jj log
    dotnet build
