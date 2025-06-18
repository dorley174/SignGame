FROM ubuntu:24.04

RUN apt-get update && apt-get install -y \
    libgtk-3-0 libnss3 libgconf-2-4 libxss1 libasound2 libx11-xcb1 libxcb-dri3-0 \
    libxcomposite1 libxcursor1 libxdamage1 libxi6 libxtst6 libgbm1 curl unzip

RUN mkdir /opt/unity
WORKDIR /opt/unity

RUN curl -L -o Unity.tar.xz "https://download.unity3d.com/download_unity/f1ef1dca8bff/LinuxEditorInstaller/Unity.tar.xz" \
    && tar -xJf Unity.tar.xz

ENV PATH="/opt/unity/Unity/Editor:${PATH}"
