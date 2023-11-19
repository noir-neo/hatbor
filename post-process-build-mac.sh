#!/bin/bash

set -e

chmod -R a+xr "${MAC_BUILD_PATH}/${PROJECT_NAME}.app"
codesign \
  --deep \
  --force \
  --verify \
  --verbose \
  --timestamp \
  --options runtime \
  --entitlements "${PROJECT_NAME}.entitlements" \
  --sign "Developer ID Application: ${APPLE_TEAM_NAME} (${APPLE_TEAM_ID})" "${MAC_BUILD_PATH}/${PROJECT_NAME}.app"

rm -rf "pkgroot"
mkdir -p "pkgroot"
cp -R "${MAC_BUILD_PATH}/${PROJECT_NAME}.app" "pkgroot/${PROJECT_NAME}.app"

pkgbuild --root "pkgroot" --component-plist "pkg-info.plist" --identifier ${BUNDLE_IDENTIFIER} --version ${VERSION} --install-location "/Applications" "${PROJECT_NAME}.pkg"

productbuild --synthesize --package "${PROJECT_NAME}.pkg" "Distribution.xml"
productbuild --distribution "Distribution.xml" --package-path . "distribution.pkg"

rm -rf "dmg-resources"
mkdir "dmg-resources"
productsign --sign "Developer ID Installer: ${APPLE_TEAM_NAME} (${APPLE_TEAM_ID})" "distribution.pkg" "dmg-resources/${PROJECT_NAME}.pkg"

hdiutil create -srcfolder "dmg-resources" -fs HFS+ -format UDZO -volname "${PROJECT_NAME}" "${PROJECT_NAME}.dmg"

xcrun notarytool store-credentials "APP_PASSWORD_NOTARIZATION" --apple-id "${APPLE_ID}" --team-id "${APPLE_TEAM_ID}" --password "${APPLE_PASSWORD}"

xcrun notarytool submit --keychain-profile "APP_PASSWORD_NOTARIZATION" --wait "${PROJECT_NAME}.dmg"

xcrun stapler staple "${PROJECT_NAME}.dmg"

