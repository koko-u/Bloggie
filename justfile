
solution_root := justfile_directory()

_default:
  @just --list
  
run:
  dotnet run --project "{{ solution_root }}/Web" --launch-profile https
  
watch:
  DOTNET_WATCH_SUPPRESS_STATIC_FILE_HANDLING=1 \
  dotnet watch run --quiet --project "{{ solution_root }}/Web" --launch-profile https
  
fmt:
  dotnet csharpier format {{ solution_root }}
  
upsubtree:
    cd {{ solution_root }}
    @if test -n "$(jj diff --stat)"; then \
        echo "Working copy has changes. Commit or move them before updating subtree."; \
        exit 1; \
    fi
    jj git export
    git subtree pull --prefix=libs/KozLibraries git@github.com:koko-u/KozLibraries.git main --squash
    jj git import
    jj log
    dotnet build
    cd {{ invocation_directory() }}
