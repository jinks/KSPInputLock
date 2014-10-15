APP			:= KSPInputLock
VERSION		:= 0.1.0

KSPDIR		:= /mnt/games/Steam/SteamApps/common/Kerbal\ Space\ Program
MANAGED		:= ${KSPDIR}/KSP_Data/Managed

GAMEDATA	:= GameData
PROJECTDIR	:= ${GAMEDATA}/${APP}
PLUGINDIR	:= ${PROJECTDIR}/Plugins
TBGAMEDATA  := ${KSPDIR}/${GAMEDATA}/000_Toolbar

TARGETS		:= ${PLUGINDIR}/KSPInputLock.dll

SRC_FILES := src/*.cs

RESGEN2	:= resgen2
GMCS	:= gmcs
GIT		:= git
TAR		:= tar
ZIP		:= zip

all: ${TARGETS}

info:
	@echo "${APP} Build Information"
	@echo "    KSP Data: ${KSPDIR}"

${TARGETS}: ${SRC_FILES}
	mkdir -p ${PLUGINDIR}
	${GMCS} -t:library -lib:${MANAGED} \
		-r:Assembly-CSharp,UnityEngine \
		-out:$@ $^

clean:
	rm -f ${TARGETS}

install: all
	rm -rf ${KSPDIR}/${PROJECTDIR}
	cp -a ${PROJECTDIR} ${KSPDIR}/${GAMEDATA}/

zip: all
	rm -f ${APP}_${VERSION}.zip
	${ZIP} -r ${APP}_${VERSION}.zip GameData README.md LICENSE -x \*.keep

.PHONY: all clean install zip
