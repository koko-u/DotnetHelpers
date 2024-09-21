output_dir := "/home/kozaki/.local/share/NuGet/Packages"

_default:
  @just --list

test:
  @if [ -d "{{output_dir}}" ]; then \
    echo '{{output_dir}}'; \
  fi

# replease packages
pack:
  @if [ ! -d "{{output_dir}}" ]; then \
    echo "no package directory - {{output_dir}}"; \
    exit 1; \
  fi
  @if !(type versionize > /dev/null 2>&1); then \
    echo "no versionize command, run 'dotnet tool install --global Versionize'"; \
    exit 1; \
  fi
  @versionize
  @for solution in **/*.sln;  do \
    dotnet pack $solution --configuration Release --output {{output_dir}}; \
  done
   