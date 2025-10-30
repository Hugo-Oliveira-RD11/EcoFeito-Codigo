{
  pkgs,
  lib,
  config,
  ...
}:
{
  name = "EcoFeito";
  # https://devenv.sh/packages/
  packages = with pkgs;[
    git
    podman
    podman-compose
  ];

  # https://devenv.sh/languages/
  languages.dotnet = {
    enable = true;
    package = pkgs.dotnet-sdk;
  };

  # See full reference at https://devenv.sh/reference/options/
}
