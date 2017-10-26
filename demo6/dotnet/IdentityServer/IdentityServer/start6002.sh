#!/usr/bin/env bash
cd  bin/Debug/netcoreapp2.0/publish
dotnet IdentityServer.dll --server.urls http://localhost:6002
