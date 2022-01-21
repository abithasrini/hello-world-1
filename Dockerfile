FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as sdk
ARG VERSION=1.0.0
WORKDIR /usr/local/src
ADD . .
RUN dotnet publish -c Release -r linux-musl-x64 --self-contained true /p:Version=${VERSION} -o /usr/local/tmp

FROM alpine
RUN apk update
RUN apk add --no-cache --no-progress libstdc++ libgcc icu
WORKDIR /usr/local/app
COPY --from=sdk /usr/local/tmp .
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV CHECK_URL=https://www.microsoft.com
CMD ["./HelloWorld"]